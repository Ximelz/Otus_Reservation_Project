using Admin.WebApi.Dtos;
using Hotels.Domain.Entities;
using Hotels.Domain.Enums;
using Hotels.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.IntegrationEvents.Hotels;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public sealed class AdminRoomsController : ControllerBase
{
    private readonly PgDbContextOptions _pgOptions;
    private readonly IPublishEndpoint _publishEndpoint;

    public AdminRoomsController(PgDbContextOptions pgOptions, IPublishEndpoint publishEndpoint)
    {
        _pgOptions = pgOptions;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("hotels/{hotelId:guid}/rooms")]
    public async Task<ActionResult<List<Dtos.RoomDto>>> GetByHotel(
        Guid hotelId,
        [FromQuery] RoomStatus? status,
        [FromQuery] HousekeepingStatus? housekeeping,
        [FromQuery] int? floor,
        [FromQuery] Guid? roomTypeId,
        CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var query = db.Rooms.AsNoTracking().Where(r => r.HotelId == hotelId);

        if (status.HasValue) query = query.Where(r => r.Status == status.Value);
        if (housekeeping.HasValue) query = query.Where(r => r.HousekeepingStatus == housekeeping.Value);
        if (floor.HasValue) query = query.Where(r => r.Floor == floor.Value);
        if (roomTypeId.HasValue) query = query.Where(r => r.TypeId == roomTypeId.Value);

        var rooms = await query.OrderBy(r => r.Floor).ThenBy(r => r.Number).ToListAsync(ct);
        var roomTypes = await db.RoomTypes.AsNoTracking().Where(rt => rt.HotelId == hotelId)
            .ToDictionaryAsync(rt => rt.Id, rt => rt.Name, ct);
        var hotel = await db.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Id == hotelId, ct);

        return Ok(rooms.Select(r => MapRoom(r, hotel?.Name ?? "", roomTypes.GetValueOrDefault(r.TypeId, ""))).ToList());
    }

    [HttpGet("rooms/{id:guid}")]
    public async Task<ActionResult<Dtos.RoomDto>> GetById(Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var room = await db.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room is null) return NotFound();
        var hotel = await db.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Id == room.HotelId, ct);
        var rtName = await db.RoomTypes.AsNoTracking().Where(rt => rt.Id == room.TypeId).Select(rt => rt.Name).FirstOrDefaultAsync(ct);
        return Ok(MapRoom(room, hotel?.Name ?? "", rtName ?? ""));
    }

    [HttpPost("hotels/{hotelId:guid}/rooms")]
    public async Task<ActionResult<Dtos.RoomDto>> Create(Guid hotelId, [FromBody] RoomUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        if (!await db.Hotels.AnyAsync(h => h.Id == hotelId, ct)) return NotFound();
        if (await db.Rooms.AnyAsync(r => r.HotelId == hotelId && r.Number == request.Number, ct))
            return Problem(statusCode: 409, title: $"Room {request.Number} already exists in this hotel");

        var room = new Room
        {
            Id = Guid.NewGuid(), HotelId = hotelId, TypeId = request.TypeId,
            Number = request.Number, Floor = request.Floor,
            Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Clean,
            ViewType = request.ViewType, Notes = request.Notes,
            IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow
        };
        db.Rooms.Add(room);
        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = hotelId, ActivityType = "RoomCreated",
            Description = $"Room {room.Number} created on floor {room.Floor}",
            EntityType = "Room", EntityId = room.Id,
            Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);
        return Ok(MapRoom(room, "", ""));
    }

    [HttpPut("rooms/{id:guid}")]
    public async Task<ActionResult<Dtos.RoomDto>> Update(Guid id, [FromBody] RoomUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room is null) return NotFound();

        room.TypeId = request.TypeId; room.Number = request.Number;
        room.Floor = request.Floor; room.ViewType = request.ViewType;
        room.Notes = request.Notes; room.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return Ok(MapRoom(room, "", ""));
    }

    [HttpPatch("rooms/{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] RoomStatusChangeRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room is null) return NotFound();

        var prev = room.Status;
        room.Status = request.Status;
        room.UpdatedAt = DateTimeOffset.UtcNow;

        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = room.HotelId, ActivityType = "RoomStatusChanged",
            Description = $"Room {room.Number}: {prev} → {request.Status}",
            EntityType = "Room", EntityId = room.Id,
            Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);

        await _publishEndpoint.Publish(new RoomStatusChangedIntegrationEvent
        {
            RoomId = room.Id,
            HotelId = room.HotelId,
            RoomNumber = room.Number,
            PreviousStatus = (Shared.Contracts.Enums.RoomStatus)(int)prev,
            NewStatus = (Shared.Contracts.Enums.RoomStatus)(int)request.Status,
            Timestamp = DateTimeOffset.UtcNow,
            CorrelationId = Guid.NewGuid()
        }, ct);

        return NoContent();
    }

    [HttpPatch("rooms/{id:guid}/housekeeping")]
    public async Task<IActionResult> ChangeHousekeeping(Guid id, [FromBody] HousekeepingStatusChangeRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room is null) return NotFound();
        var hotelName = await db.Hotels.Where(h => h.Id == room.HotelId).Select(h => h.Name).FirstOrDefaultAsync(ct) ?? "";

        var prev = room.HousekeepingStatus;
        room.HousekeepingStatus = request.HousekeepingStatus;
        room.UpdatedAt = DateTimeOffset.UtcNow;

        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = room.HotelId, ActivityType = "HousekeepingChanged",
            Description = $"Room {room.Number}: {prev} → {request.HousekeepingStatus}",
            EntityType = "Room", EntityId = room.Id,
            Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);

        await _publishEndpoint.Publish(new HousekeepingStatusChangedIntegrationEvent
        {
            RoomId = room.Id,
            HotelId = room.HotelId,
            RoomNumber = room.Number,
            HotelName = hotelName,
            PreviousStatus = (Shared.Contracts.Enums.HousekeepingStatus)(int)prev,
            NewStatus = (Shared.Contracts.Enums.HousekeepingStatus)(int)request.HousekeepingStatus,
            Timestamp = DateTimeOffset.UtcNow,
            CorrelationId = Guid.NewGuid()
        }, ct);

        return NoContent();
    }

    [HttpPatch("rooms/bulk-housekeeping")]
    public async Task<IActionResult> BulkHousekeeping([FromBody] BulkHousekeepingRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var rooms = await db.Rooms.Where(r => request.RoomIds.Contains(r.Id)).ToListAsync(ct);
        var hotelIds = rooms.Select(r => r.HotelId).Distinct().ToList();
        var hotelNames = await db.Hotels.Where(h => hotelIds.Contains(h.Id)).ToDictionaryAsync(h => h.Id, h => h.Name, ct);
        var previousStatuses = rooms.ToDictionary(r => r.Id, r => r.HousekeepingStatus);
        foreach (var room in rooms)
        {
            room.HousekeepingStatus = request.HousekeepingStatus;
            room.UpdatedAt = DateTimeOffset.UtcNow;
        }
        await db.SaveChangesAsync(ct);

        foreach (var room in rooms)
        {
            var prevStatus = previousStatuses[room.Id];
            await _publishEndpoint.Publish(new HousekeepingStatusChangedIntegrationEvent
            {
                RoomId = room.Id,
                HotelId = room.HotelId,
                RoomNumber = room.Number,
                HotelName = hotelNames.GetValueOrDefault(room.HotelId, ""),
                PreviousStatus = (Shared.Contracts.Enums.HousekeepingStatus)(int)prevStatus,
                NewStatus = (Shared.Contracts.Enums.HousekeepingStatus)(int)request.HousekeepingStatus,
                Timestamp = DateTimeOffset.UtcNow,
                CorrelationId = Guid.NewGuid()
            }, ct);
        }

        return NoContent();
    }

    [HttpDelete("rooms/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        await db.Rooms.Where(r => r.Id == id).ExecuteDeleteAsync(ct);
        return NoContent();
    }

    private static Dtos.RoomDto MapRoom(Room r, string hotelName, string roomTypeName) => new()
    {
        Id = r.Id, HotelId = r.HotelId, HotelName = hotelName,
        TypeId = r.TypeId, RoomTypeName = roomTypeName,
        Number = r.Number, Floor = r.Floor,
        Status = r.Status, HousekeepingStatus = r.HousekeepingStatus,
        ViewType = r.ViewType, Notes = r.Notes, IsActive = r.IsActive
    };
}
