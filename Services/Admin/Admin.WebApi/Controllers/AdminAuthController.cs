using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Admin.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Admin.WebApi.Controllers;

[ApiController]
[Route("api/admin/auth")]
public sealed class AdminAuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AdminAuthController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<AdminLoginResponse> Login([FromBody] AdminLoginRequest request)
    {
        // Проверка окружения
        if (!_environment.IsDevelopment())
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "DevAuth доступен только в Development окружении",
                detail: $"Текущее окружение: {_environment.EnvironmentName}");
        }

        var enabled = _configuration.GetValue<bool>("Admin:DevAuth:Enabled");
        if (!enabled)
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "DevAuth отключен",
                detail: "Установите Admin:DevAuth:Enabled=true в appsettings.Development.json");
        }

        var expectedUsername = _configuration["Admin:DevAuth:Username"];
        var expectedPassword = _configuration["Admin:DevAuth:Password"];

        if (string.IsNullOrWhiteSpace(expectedUsername) || string.IsNullOrWhiteSpace(expectedPassword))
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "DevAuth включен, но credentials не настроены",
                detail: "Настройте Admin:DevAuth:Username и Admin:DevAuth:Password в appsettings.Development.json");
        }

        // Валидация входных данных
        if (string.IsNullOrWhiteSpace(request?.Username) || string.IsNullOrWhiteSpace(request?.Password))
        {
            return BadRequest(new { error = "Логин и пароль обязательны" });
        }

        // Проверка credentials
        if (!string.Equals(request.Username, expectedUsername, StringComparison.Ordinal) ||
            !string.Equals(request.Password, expectedPassword, StringComparison.Ordinal))
        {
            return Unauthorized(new { error = "Неверный логин или пароль" });
        }

        var signingKey = _configuration["Admin:Jwt:SigningKey"] ?? "dev-signing-key-change-me-32chars-minimum";
        var issuer = _configuration["Admin:Jwt:Issuer"] ?? "Admin.WebApi";
        var audience = _configuration["Admin:Jwt:Audience"] ?? "Admin.WebApi";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTimeOffset.UtcNow;
        var expires = now.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, request.Username ?? "admin"),
                new Claim(ClaimTypes.Name, request.Username ?? "admin"),
                new Claim(ClaimTypes.Role, "Admin")
            ],
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new AdminLoginResponse
        {
            AccessToken = tokenString,
            ExpiresInSeconds = (int)(expires - now).TotalSeconds
        });
    }
}
