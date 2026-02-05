using Admin.Application.Abstractions;
using Admin.Application.Services;
using Admin.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin")]
public sealed class AdminDashboardController : ControllerBase
{
    private readonly SystemSettingsService _settingsService;
    private readonly IHotelsClient _hotelsClient;

    public AdminDashboardController(SystemSettingsService settingsService, IHotelsClient hotelsClient)
    {
        _settingsService = settingsService;
        _hotelsClient = hotelsClient;
    }

    [HttpGet]
    public async Task<ActionResult<AdminDashboardResponse>> Get([FromQuery] int? countryId, [FromQuery] int? stars, CancellationToken cancellationToken)
    {
        if (countryId.HasValue && stars.HasValue)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Specify either countryId or stars");
        }

        var settings = await _settingsService.GetAll(cancellationToken);
        var warnings = new List<string>();

        int? hotelsCount = null;
        string? hotelsFilter = null;

        try
        {
            if (countryId.HasValue)
            {
                var hotels = await _hotelsClient.GetHotelsByCountry(countryId.Value, cancellationToken);
                hotelsCount = hotels.Count;
                hotelsFilter = $"countryId={countryId.Value}";
            }
            else if (stars.HasValue)
            {
                var hotels = await _hotelsClient.GetHotelsByStars(stars.Value, cancellationToken);
                hotelsCount = hotels.Count;
                hotelsFilter = $"stars={stars.Value}";
            }
            else
            {
                warnings.Add("Hotels summary is not calculated: provide countryId or stars.");
            }
        }
        catch (Exception ex)
        {
            warnings.Add($"Hotels summary unavailable: {ex.Message}");
        }

        return Ok(new AdminDashboardResponse
        {
            GeneratedAt = DateTimeOffset.UtcNow,
            Settings = settings,
            HotelsCount = hotelsCount,
            HotelsFilter = hotelsFilter,
            Warnings = warnings
        });
    }
}

