using System.Net;
using Admin.Application.Abstractions;
using Admin.Application.Contracts.Hotels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/rooms")]
[Authorize(Roles = "Admin")]
public sealed class AdminRoomsController : ControllerBase
{
    private readonly IHotelsClient _hotelsClient;

    public AdminRoomsController(IHotelsClient hotelsClient)
    {
        _hotelsClient = hotelsClient;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RoomDto>> Get(long id, CancellationToken cancellationToken)
    {
        try
        {
            var room = await _hotelsClient.GetRoom(id, cancellationToken);
            return room is null ? NotFound() : Ok(room);
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpGet("hotel/{hotelId:long}")]
    public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetByHotel(long hotelId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _hotelsClient.GetRoomsByHotel(hotelId, cancellationToken));
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] RoomDto room, CancellationToken cancellationToken)
    {
        try
        {
            await _hotelsClient.UpsertRoom(room, cancellationToken);
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
            await _hotelsClient.DeleteRoom(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return MapDownstreamError("HotelService", ex);
        }
    }

    [HttpDelete("hotel/{hotelId:long}")]
    public async Task<IActionResult> DeleteByHotel(long hotelId, CancellationToken cancellationToken)
    {
        try
        {
            await _hotelsClient.DeleteRoomsByHotel(hotelId, cancellationToken);
            return NoContent();
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

