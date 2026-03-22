using Admin.WebApi.Dtos;
using Hotels.Domain.Entities;
using Hotels.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/hotels/{hotelId:guid}/rate-plans")]
[Authorize(Roles = "Admin")]
public sealed class AdminRatePlansController : ControllerBase
{
    private readonly PgDbContextOptions _pgOptions;
    public AdminRatePlansController(PgDbContextOptions pgOptions) => _pgOptions = pgOptions;

    [HttpGet]
    public async Task<ActionResult<List<RatePlanDto>>> GetAll(Guid hotelId, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var plans = await db.RatePlans.AsNoTracking()
            .Where(rp => rp.HotelId == hotelId)
            .OrderBy(rp => rp.Name)
            .ToListAsync(ct);

        var roomTypes = await db.RoomTypes.AsNoTracking()
            .Where(rt => rt.HotelId == hotelId)
            .ToDictionaryAsync(rt => rt.Id, rt => rt.Name, ct);

        return Ok(plans.Select(rp => MapToDto(rp, roomTypes.GetValueOrDefault(rp.RoomTypeId, ""))).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<RatePlanDto>> Create(Guid hotelId, [FromBody] RatePlanUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        if (!await db.Hotels.AnyAsync(h => h.Id == hotelId, ct)) return NotFound();

        var rp = new RatePlan
        {
            Id = Guid.NewGuid(), HotelId = hotelId, RoomTypeId = request.RoomTypeId,
            Code = request.Code, Name = request.Name, BasePrice = request.BasePrice,
            Currency = request.Currency, CancellationPolicyType = request.CancellationPolicyType,
            BreakfastIncluded = request.BreakfastIncluded, PrepaymentRequired = request.PrepaymentRequired,
            IsDefault = request.IsDefault, IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow
        };
        db.RatePlans.Add(rp);
        db.RecentActivityLogs.Add(new RecentActivityLog
        {
            Id = Guid.NewGuid(), HotelId = hotelId, ActivityType = "RatePlanCreated",
            Description = $"Rate plan '{rp.Name}' created", EntityType = "RatePlan",
            EntityId = rp.Id, Timestamp = DateTimeOffset.UtcNow, PerformedBy = User.Identity?.Name
        });
        await db.SaveChangesAsync(ct);
        return Ok(MapToDto(rp, ""));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RatePlanDto>> Update(Guid hotelId, Guid id, [FromBody] RatePlanUpsertRequest request, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        var rp = await db.RatePlans.FirstOrDefaultAsync(r => r.Id == id && r.HotelId == hotelId, ct);
        if (rp is null) return NotFound();

        rp.RoomTypeId = request.RoomTypeId; rp.Code = request.Code; rp.Name = request.Name;
        rp.BasePrice = request.BasePrice; rp.Currency = request.Currency;
        rp.CancellationPolicyType = request.CancellationPolicyType;
        rp.BreakfastIncluded = request.BreakfastIncluded; rp.PrepaymentRequired = request.PrepaymentRequired;
        rp.IsDefault = request.IsDefault; rp.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
        return Ok(MapToDto(rp, ""));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid hotelId, Guid id, CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        await db.RatePlans.Where(rp => rp.Id == id && rp.HotelId == hotelId).ExecuteDeleteAsync(ct);
        return NoContent();
    }

    private static RatePlanDto MapToDto(RatePlan rp, string roomTypeName) => new()
    {
        Id = rp.Id, HotelId = rp.HotelId, RoomTypeId = rp.RoomTypeId,
        RoomTypeName = roomTypeName, Code = rp.Code, Name = rp.Name,
        BasePrice = rp.BasePrice, Currency = rp.Currency,
        CancellationPolicyType = rp.CancellationPolicyType,
        BreakfastIncluded = rp.BreakfastIncluded, PrepaymentRequired = rp.PrepaymentRequired,
        IsDefault = rp.IsDefault, IsActive = rp.IsActive
    };
}
