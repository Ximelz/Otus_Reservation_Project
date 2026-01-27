namespace Admin.Application.Contracts.Hotels;

public sealed record HotelDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Stars { get; init; }
    public string Description { get; init; } = string.Empty;
    public int CountryId { get; init; }
    public string Address { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

