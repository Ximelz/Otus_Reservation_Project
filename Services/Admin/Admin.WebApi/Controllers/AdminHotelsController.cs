using Admin.WebApi.Dtos;
using Hotels.Domain.Entities;
using Hotels.Domain.Enums;
using Hotels.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/hotels")]
[Authorize(Roles = "Admin")]
public sealed class AdminHotelsController : ControllerBase
{
    private readonly PgDbContextOptions _pgOptions;

    public AdminHotelsController(PgDbContextOptions pgOptions)
    {
        _pgOptions = pgOptions;
    }

    [HttpGet]
    public async Task<ActionResult<List<HotelListItemDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery] int? stars,
        CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var query = db.Hotels.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(h => h.Name.ToLower().Contains(search.ToLower()) ||
                                     h.Slug.ToLower().Contains(search.ToLower()) ||
                                     h.City.ToLower().Contains(search.ToLower()));
        if (isActive.HasValue)
            query = query.Where(h => h.IsActive == isActive.Value);
        if (stars.HasValue)
            query = query.Where(h => h.Stars == stars.Value);

        var hotels = await query.OrderBy(h => h.Name).ToListAsync(ct);
        var roomCounts = await db.Rooms.AsNoTracking()
            .GroupBy(r => r.HotelId)
            .Select(g => new { HotelId = g.Key, Total = g.Count(), Available = g.Count(r => r.Status == RoomStatus.Available && r.IsActive) })
            .ToDictionaryAsync(x => x.HotelId, ct);

        var result = hotels.Select(h =>
        {
            roomCounts.TryGetValue(h.Id, out var rc);
            return new HotelListItemDto
            {
                Id = h.Id, Name = h.Name, Slug = h.Slug, City = h.City,
                Stars = h.Stars, IsActive = h.IsActive,
                TotalRooms = rc?.Total ?? 0, AvailableRooms = rc?.Available ?? 0,
                CreatedAt = h.CreatedAt
            };
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HotelDetailsDto>> GetById(Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var hotel = await db.Hotels.AsNoTracking()
            .Include(h => h.Amenities)
            .Include(h => h.Policy)
            .FirstOrDefaultAsync(h => h.Id == id, ct);

        if (hotel is null) return NotFound();
        return Ok(MapToDetails(hotel));
    }

    [HttpPost]
    public async Task<ActionResult<HotelDetailsDto>> Create([FromBody] HotelUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var slug = request.Slug ?? GenerateSlug(request.Name);
        if (await db.Hotels.AnyAsync(h => h.Slug == slug, ct))
            return Problem(statusCode: 409, title: "Hotel with this slug already exists");

        var hotel = new Hotel
        {
            Id = Guid.NewGuid(), Name = request.Name, Slug = slug,
            Stars = request.Stars, Description = request.Description,
            City = request.City, CountryId = request.CountryId,
            Address = request.Address, Timezone = request.Timezone,
            CheckInTime = TimeOnly.Parse(request.CheckInTime),
            CheckOutTime = TimeOnly.Parse(request.CheckOutTime),
            Phone = request.Phone, Email = request.Email,
            IsActive = true, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow
        };
        db.Hotels.Add(hotel);
        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "HotelCreated",
            Description = $"Hotel '{hotel.Name}' created", EntityType = "Hotel",
            EntityId = hotel.Id, Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, MapToDetails(hotel));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<HotelDetailsDto>> Update(Guid id, [FromBody] HotelUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var hotel = await db.Hotels.Include(h => h.Amenities).Include(h => h.Policy).FirstOrDefaultAsync(h => h.Id == id, ct);
        if (hotel is null) return NotFound();

        hotel.Name = request.Name;
        hotel.Slug = request.Slug ?? hotel.Slug;
        hotel.Stars = request.Stars;
        hotel.Description = request.Description;
        hotel.City = request.City;
        hotel.CountryId = request.CountryId;
        hotel.Address = request.Address;
        hotel.Timezone = request.Timezone;
        hotel.CheckInTime = TimeOnly.Parse(request.CheckInTime);
        hotel.CheckOutTime = TimeOnly.Parse(request.CheckOutTime);
        hotel.Phone = request.Phone;
        hotel.Email = request.Email;
        hotel.UpdatedAt = DateTimeOffset.UtcNow;
        hotel.UpdatedBy = User.Identity?.Name;

        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "HotelUpdated",
            Description = $"Hotel '{hotel.Name}' updated", EntityType = "Hotel",
            EntityId = hotel.Id, Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);
        return Ok(MapToDetails(hotel));
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var hotel = await db.Hotels.FindAsync([id], ct);
        if (hotel is null) return NotFound();
        hotel.IsActive = false;
        hotel.UpdatedAt = DateTimeOffset.UtcNow;
        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "HotelDeactivated",
            Description = $"Hotel '{hotel.Name}' deactivated", EntityType = "Hotel",
            EntityId = hotel.Id, Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var hotel = await db.Hotels.FindAsync([id], ct);
        if (hotel is null) return NotFound();
        hotel.IsActive = true;
        hotel.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpPost("{hotelId:guid}/amenities")]
    public async Task<ActionResult<HotelAmenityDto>> AddAmenity(Guid hotelId, [FromBody] HotelAmenityUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        if (!await db.Hotels.AnyAsync(h => h.Id == hotelId, ct)) return NotFound();
        var amenity = new HotelAmenity
        {
            Id = Guid.NewGuid(), HotelId = hotelId,
            AmenityType = request.AmenityType, CustomName = request.CustomName, IsAvailable = request.IsAvailable
        };
        db.HotelAmenities.Add(amenity);
        await db.SaveChangesAsync(ct);
        return Ok(new HotelAmenityDto { Id = amenity.Id, AmenityType = amenity.AmenityType, CustomName = amenity.CustomName, IsAvailable = amenity.IsAvailable });
    }

    [HttpDelete("{hotelId:guid}/amenities/{amenityId:guid}")]
    public async Task<IActionResult> RemoveAmenity(Guid hotelId, Guid amenityId, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        await db.HotelAmenities.Where(a => a.Id == amenityId && a.HotelId == hotelId).ExecuteDeleteAsync(ct);
        return NoContent();
    }

    [HttpPut("{hotelId:guid}/policy")]
    public async Task<ActionResult<HotelPolicyDto>> UpsertPolicy(Guid hotelId, [FromBody] PolicyUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        if (!await db.Hotels.AnyAsync(h => h.Id == hotelId, ct)) return NotFound();
        var policy = await db.HotelPolicies.FirstOrDefaultAsync(p => p.HotelId == hotelId, ct);
        if (policy is null)
        {
            policy = new HotelPolicy { Id = Guid.NewGuid(), HotelId = hotelId };
            db.HotelPolicies.Add(policy);
        }
        policy.CheckInTime = TimeOnly.Parse(request.CheckInTime);
        policy.CheckOutTime = TimeOnly.Parse(request.CheckOutTime);
        policy.EarlyCheckInNote = request.EarlyCheckInNote;
        policy.LateCheckOutNote = request.LateCheckOutNote;
        policy.CancellationPolicyText = request.CancellationPolicyText;
        policy.ConfirmationPendingEnabled = request.ConfirmationPendingEnabled;
        policy.AutoConfirmRules = request.AutoConfirmRules;
        policy.TermsAndConditionsText = request.TermsAndConditionsText;
        policy.ContactInstructions = request.ContactInstructions;
        policy.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return Ok(MapPolicy(policy));
    }

    private static HotelDetailsDto MapToDetails(Hotel h) => new()
    {
        Id = h.Id, Name = h.Name, Slug = h.Slug, Stars = h.Stars,
        Description = h.Description, City = h.City, CountryId = h.CountryId,
        Address = h.Address, Timezone = h.Timezone,
        CheckInTime = h.CheckInTime.ToString("HH:mm"),
        CheckOutTime = h.CheckOutTime.ToString("HH:mm"),
        Phone = h.Phone, Email = h.Email, IsActive = h.IsActive,
        CreatedAt = h.CreatedAt, UpdatedAt = h.UpdatedAt,
        Amenities = h.Amenities?.Select(a => new HotelAmenityDto { Id = a.Id, AmenityType = a.AmenityType, CustomName = a.CustomName, IsAvailable = a.IsAvailable }).ToList() ?? [],
        Policy = h.Policy != null ? MapPolicy(h.Policy) : null
    };

    private static HotelPolicyDto MapPolicy(HotelPolicy p) => new()
    {
        Id = p.Id, CheckInTime = p.CheckInTime.ToString("HH:mm"), CheckOutTime = p.CheckOutTime.ToString("HH:mm"),
        EarlyCheckInNote = p.EarlyCheckInNote, LateCheckOutNote = p.LateCheckOutNote,
        CancellationPolicyText = p.CancellationPolicyText,
        ConfirmationPendingEnabled = p.ConfirmationPendingEnabled, AutoConfirmRules = p.AutoConfirmRules,
        TermsAndConditionsText = p.TermsAndConditionsText, ContactInstructions = p.ContactInstructions
    };

    private static string GenerateSlug(string name) =>
        name.ToLowerInvariant().Replace(' ', '-').Replace("'", "").Replace("\"", "");
}
