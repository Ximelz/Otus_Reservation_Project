namespace Admin.Ui.Services;

public sealed class AdminApiOptions
{
    public string BaseUrl { get; set; } = "http://localhost:5009";
    public string DevUsername { get; set; } = "admin";
    public string DevPassword { get; set; } = "change-me";
}

