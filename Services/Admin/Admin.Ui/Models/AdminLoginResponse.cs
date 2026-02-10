namespace Admin.Ui.Models;

public sealed record AdminLoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string TokenType { get; init; } = "Bearer";
    public int ExpiresInSeconds { get; init; }
}

