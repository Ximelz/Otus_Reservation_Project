using Admin.WebApi.Dtos;
using Hotels.Domain.Enums;
using Hotels.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin")]
public sealed class AdminDashboardController : ControllerBase
{
    private readonly PgDbContextOptions _pgOptions;
    public AdminDashboardController(PgDbContextOptions pgOptions) => _pgOptions = pgOptions;

    [HttpGet]
    public async Task<ActionResult<DashboardSummaryDto>> Get(CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);

        var hotels = await db.Hotels.AsNoTracking().Where(h => h.IsActive).ToListAsync(ct);
        var rooms = await db.Rooms.AsNoTracking().ToListAsync(ct);
        var roomTypes = await db.RoomTypes.AsNoTracking().ToDictionaryAsync(rt => rt.Id, rt => rt.Name, ct);

        var totalRooms = rooms.Count;
        var available = rooms.Count(r => r.Status == RoomStatus.Available && r.IsActive);
        var occupied = rooms.Count(r => r.Status == RoomStatus.Occupied);
        var reserved = rooms.Count(r => r.Status == RoomStatus.Reserved);
        var dirty = rooms.Count(r => r.HousekeepingStatus == HousekeepingStatus.Dirty);
        var outOfService = rooms.Count(r => r.Status == RoomStatus.OutOfService);
        var maintenance = rooms.Count(r => r.Status == RoomStatus.Maintenance);
        var activeRooms = rooms.Count(r => r.IsActive);
        var occupancyRate = activeRooms > 0 ? Math.Round((decimal)occupied / activeRooms * 100, 1) : 0;

        var topRoomTypes = rooms
            .GroupBy(r => r.TypeId)
            .Select(g => new RoomTypeOccupancyDto
            {
                RoomTypeName = roomTypes.GetValueOrDefault(g.Key, "Unknown"),
                TotalRooms = g.Count(),
                OccupiedRooms = g.Count(r => r.Status == RoomStatus.Occupied),
                OccupancyRate = g.Count() > 0 ? Math.Round((decimal)g.Count(r => r.Status == RoomStatus.Occupied) / g.Count() * 100, 1) : 0
            })
            .OrderByDescending(rt => rt.OccupancyRate)
            .Take(5)
            .ToList();

        var hotelComparisons = hotels.Select(h =>
        {
            var hotelRooms = rooms.Where(r => r.HotelId == h.Id).ToList();
            var hotelActive = hotelRooms.Count(r => r.IsActive);
            var hotelOccupied = hotelRooms.Count(r => r.Status == RoomStatus.Occupied);
            return new HotelComparisonDto
            {
                HotelId = h.Id, HotelName = h.Name, Stars = h.Stars,
                TotalRooms = hotelRooms.Count,
                AvailableRooms = hotelRooms.Count(r => r.Status == RoomStatus.Available && r.IsActive),
                OccupiedRooms = hotelOccupied,
                OccupancyRate = hotelActive > 0 ? Math.Round((decimal)hotelOccupied / hotelActive * 100, 1) : 0,
                DirtyRooms = hotelRooms.Count(r => r.HousekeepingStatus == HousekeepingStatus.Dirty),
                IsActive = h.IsActive
            };
        }).ToList();

        var recentActivity = await db.RecentActivityLogs.AsNoTracking()
            .OrderByDescending(a => a.Timestamp)
            .Take(20)
            .Select(a => new RecentActivityItemDto
            {
                Id = a.Id, HotelId = a.HotelId, ActivityType = a.ActivityType,
                Description = a.Description, EntityType = a.EntityType,
                EntityId = a.EntityId, Timestamp = a.Timestamp, PerformedBy = a.PerformedBy
            })
            .ToListAsync(ct);

        return Ok(new DashboardSummaryDto
        {
            TotalHotels = hotels.Count, TotalRooms = totalRooms,
            AvailableRooms = available, OccupiedRooms = occupied,
            ReservedRooms = reserved, DirtyRooms = dirty,
            OutOfServiceRooms = outOfService, MaintenanceRooms = maintenance,
            OccupancyRate = occupancyRate,
            TopRoomTypesByOccupancy = topRoomTypes,
            HotelComparisons = hotelComparisons,
            RecentActivity = recentActivity,
            GeneratedAt = DateTimeOffset.UtcNow
        });
    }

    [HttpGet("housekeeping")]
    public async Task<ActionResult<HousekeepingBoardDto>> GetHousekeeping([FromQuery] Guid hotelId, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var hotel = await db.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Id == hotelId, ct);
        if (hotel is null) return NotFound();

        var rooms = await db.Rooms.AsNoTracking()
            .Where(r => r.HotelId == hotelId)
            .OrderBy(r => r.Floor).ThenBy(r => r.Number)
            .ToListAsync(ct);

        var roomTypes = await db.RoomTypes.AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .ToDictionaryAsync(rt => rt.Id, rt => rt.Name, ct);

        var floors = rooms
            .GroupBy(r => r.Floor)
            .OrderBy(g => g.Key)
            .Select(g => new FloorGroupDto
            {
                Floor = g.Key,
                Rooms = g.Select(r => new Dtos.RoomDto
                {
                    Id = r.Id, HotelId = r.HotelId, HotelName = hotel.Name,
                    TypeId = r.TypeId, RoomTypeName = roomTypes.GetValueOrDefault(r.TypeId, ""),
                    Number = r.Number, Floor = r.Floor,
                    Status = r.Status, HousekeepingStatus = r.HousekeepingStatus,
                    ViewType = r.ViewType, Notes = r.Notes, IsActive = r.IsActive
                }).ToList()
            }).ToList();

        return Ok(new HousekeepingBoardDto
        {
            HotelId = hotel.Id, HotelName = hotel.Name, Floors = floors,
            Summary = new HousekeepingSummaryDto
            {
                TotalRooms = rooms.Count,
                CleanRooms = rooms.Count(r => r.HousekeepingStatus == HousekeepingStatus.Clean),
                DirtyRooms = rooms.Count(r => r.HousekeepingStatus == HousekeepingStatus.Dirty),
                InspectedRooms = rooms.Count(r => r.HousekeepingStatus == HousekeepingStatus.Inspected),
                OutOfServiceRooms = rooms.Count(r => r.Status == RoomStatus.OutOfService)
            }
        });
    }

    [HttpGet("activity")]
    public async Task<ActionResult<List<RecentActivityItemDto>>> GetActivity(
        [FromQuery] Guid? hotelId, [FromQuery] int limit = 50, CancellationToken ct = default)
    {
        using var db = new PgDbContext(_pgOptions);
        var query = db.RecentActivityLogs.AsNoTracking().AsQueryable();
        if (hotelId.HasValue)
            query = query.Where(a => a.HotelId == hotelId.Value);

        return Ok(await query
            .OrderByDescending(a => a.Timestamp)
            .Take(limit)
            .Select(a => new RecentActivityItemDto
            {
                Id = a.Id, HotelId = a.HotelId, ActivityType = a.ActivityType,
                Description = a.Description, EntityType = a.EntityType,
                EntityId = a.EntityId, Timestamp = a.Timestamp, PerformedBy = a.PerformedBy
            })
            .ToListAsync(ct));
    }
}
