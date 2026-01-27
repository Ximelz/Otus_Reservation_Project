namespace Admin.Ui.Models;

public sealed record AdminLoginRequest
{
    public string? Username { get; init; }
    public string? Password { get; init; }
}

