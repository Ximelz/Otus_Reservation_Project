namespace Admin.WebApi.Models;

public sealed record UpsertSystemSettingRequest
{
    public string Value { get; init; } = string.Empty;
}

