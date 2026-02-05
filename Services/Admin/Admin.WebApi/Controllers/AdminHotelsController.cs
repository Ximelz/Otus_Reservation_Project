using System.Net;
using Admin.Application.Abstractions;
using Admin.Application.Contracts.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/hotels")]
[Authorize(Roles = "Admin")]
public sealed class AdminHotelsController : ControllerBase
{
    private readonly IHotelsClient _hotelsClient;

    public AdminHotelsController(IHotelsClient hotelsClient)
    {
        _hotelsClient = hotelsClient;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<HotelDto>> Get(long id, CancellationToken cancellationToken)
    {
        try
        {
            var hotel = await _hotelsClient.GetHotel(id, cancellationToken);
            return hotel is null ? NotFound() : Ok(hotel);
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] HotelDto hotel, CancellationToken cancellationToken)
    {
        try
        {
            await _hotelsClient.UpsertHotel(hotel, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _hotelsClient.DeleteHotel(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpGet("byCountry/{countryId:int}")]
    public async Task<ActionResult<IReadOnlyList<HotelDto>>> GetByCountry(int countryId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _hotelsClient.GetHotelsByCountry(countryId, cancellationToken));
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpGet("byStars/{stars:int}")]
    public async Task<ActionResult<IReadOnlyList<HotelDto>>> GetByStars(int stars, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _hotelsClient.GetHotelsByStars(stars, cancellationToken));
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    private ObjectResult MapDownstreamError(string dependencyName, Exception exception)
    {
        if (exception is TaskCanceledException)
        {
            return Problem(statusCode: StatusCodes.Status504GatewayTimeout, title: $"{dependencyName} timeout");
        }

        if (exception is HttpRequestException httpEx)
        {
            if (httpEx.StatusCode is HttpStatusCode statusCode)
            {
                return Problem(statusCode: (int)statusCode, title: $"{dependencyName} error", detail: httpEx.Message);
            }

            return Problem(statusCode: StatusCodes.Status503ServiceUnavailable, title: $"{dependencyName} unavailable", detail: httpEx.Message);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Unexpected error", detail: exception.Message);
    }
}

