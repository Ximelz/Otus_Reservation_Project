using Admin.Application.Models;
using Admin.Application.Services;
using Admin.Domain.Entities;
using Admin.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/settings")]
[Authorize(Roles = "Admin")]
public sealed class AdminSettingsController : ControllerBase
{
    private readonly SystemSettingsService _service;

    public AdminSettingsController(SystemSettingsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SystemSetting>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _service.GetAll(cancellationToken));
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<SystemSetting>> GetByKey(string key, CancellationToken cancellationToken)
    {
        try
        {
            var setting = await _service.GetByKey(key, cancellationToken);
            return setting is null ? NotFound() : Ok(setting);
        }
        catch (ArgumentException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid setting key", detail: ex.Message);
        }
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> Upsert(string key, [FromBody] UpsertSystemSettingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var upsertResult = await _service.Upsert(key, request.Value, cancellationToken);
            var setting = await _service.GetByKey(key, cancellationToken);

            if (setting is null)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Upsert failed");
            }

            return upsertResult == SystemSettingUpsertResult.Created
                ? CreatedAtAction(nameof(GetByKey), new { key }, setting)
                : Ok(setting);
        }
        catch (ArgumentException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid request", detail: ex.Message);
        }
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.Delete(key, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid setting key", detail: ex.Message);
        }
    }
}
