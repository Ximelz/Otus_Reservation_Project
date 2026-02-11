namespace Admin.Ui.Models;

public sealed record AdminDashboardResponse
{
    public DateTimeOffset GeneratedAt { get; init; }
    public IReadOnlyList<SystemSetting> Settings { get; init; } = [];
    public int? HotelsCount { get; init; }
    public string? HotelsFilter { get; init; }
    public IReadOnlyList<string> Warnings { get; init; } = [];
}

