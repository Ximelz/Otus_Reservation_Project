namespace Admin.Application.Contracts.Hotels;

public sealed record RoomDto
{
    public long Id { get; init; }
    public long HotelId { get; init; }
    public string Number { get; init; } = string.Empty;
    public int Capacity { get; init; } = 1;
    public int ComfortLevel { get; init; } = 1;
    public decimal Price { get; init; }
}

