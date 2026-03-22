using Admin.WebApi.Dtos;
using Hotels.Domain.Entities;
using Hotels.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/hotels/{hotelId:guid}/room-types")]
[Authorize(Roles = "Admin")]
public sealed class AdminRoomTypesController : ControllerBase
{
    private readonly PgDbContextOptions _pgOptions;
    public AdminRoomTypesController(PgDbContextOptions pgOptions) => _pgOptions = pgOptions;

    [HttpGet]
    public async Task<ActionResult<List<RoomTypeDto>>> GetAll(Guid hotelId, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var types = await db.RoomTypes.AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .OrderBy(rt => rt.Name)
            .ToListAsync(ct);

        var roomCounts = await db.Rooms.AsNoTracking()
            .Where(r => r.HotelId == hotelId)
            .GroupBy(r => r.TypeId)
            .Select(g => new { TypeId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.TypeId, x => x.Count, ct);

        return Ok(types.Select(rt => MapToDto(rt, roomCounts.GetValueOrDefault(rt.Id))).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomTypeDto>> GetById(Guid hotelId, Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var rt = await db.RoomTypes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id && r.HotelId == hotelId, ct);
        if (rt is null) return NotFound();
        return Ok(MapToDto(rt, 0));
    }

    [HttpPost]
    public async Task<ActionResult<RoomTypeDto>> Create(Guid hotelId, [FromBody] RoomTypeUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        if (!await db.Hotels.AnyAsync(h => h.Id == hotelId, ct)) return NotFound();

        var rt = new RoomType
        {
            Id = Guid.NewGuid(), HotelId = hotelId, Code = request.Code, Name = request.Name,
            Description = request.Description, Capacity = request.Capacity,
            CapacityAdults = request.CapacityAdults, CapacityChildren = request.CapacityChildren,
            BedConfiguration = request.BedConfiguration, BaseAreaSqm = request.BaseAreaSqm,
            IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow
        };
        db.RoomTypes.Add(rt);
        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = hotelId, ActivityType = "RoomTypeCreated",
            Description = $"Room type '{rt.Name}' created", EntityType = "RoomType",
            EntityId = rt.Id, Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);
        return Ok(MapToDto(rt, 0));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RoomTypeDto>> Update(Guid hotelId, Guid id, [FromBody] RoomTypeUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var rt = await db.RoomTypes.FirstOrDefaultAsync(r => r.Id == id && r.HotelId == hotelId, ct);
        if (rt is null) return NotFound();

        rt.Code = request.Code; rt.Name = request.Name; rt.Description = request.Description;
        rt.Capacity = request.Capacity; rt.CapacityAdults = request.CapacityAdults;
        rt.CapacityChildren = request.CapacityChildren; rt.BedConfiguration = request.BedConfiguration;
        rt.BaseAreaSqm = request.BaseAreaSqm; rt.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return Ok(MapToDto(rt, 0));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid hotelId, Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        await db.RoomTypes.Where(rt => rt.Id == id && rt.HotelId == hotelId).ExecuteDeleteAsync(ct);
        return NoContent();
    }

    private static RoomTypeDto MapToDto(RoomType rt, int roomCount) => new()
    {
        Id = rt.Id, HotelId = rt.HotelId, Code = rt.Code, Name = rt.Name,
        Description = rt.Description, Capacity = rt.Capacity,
        CapacityAdults = rt.CapacityAdults, CapacityChildren = rt.CapacityChildren,
        BedConfiguration = rt.BedConfiguration, BaseAreaSqm = rt.BaseAreaSqm,
        IsActive = rt.IsActive, RoomCount = roomCount
    };
}
