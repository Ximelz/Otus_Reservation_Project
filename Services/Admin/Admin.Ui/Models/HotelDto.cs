namespace Admin.Ui.Models;

public sealed record HotelDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string Description { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
