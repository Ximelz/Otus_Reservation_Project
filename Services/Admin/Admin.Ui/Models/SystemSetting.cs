namespace Admin.Ui.Models;

public sealed record SystemSetting
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; init; }
}

