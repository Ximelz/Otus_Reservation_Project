namespace Admin.Ui.Models;

public sealed record RoomDto
{
    public long Id { get; set; }
    public long HotelId { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Capacity { get; set; } = 1;
    public int ComfortLevel { get; set; } = 1;
    public decimal Price { get; set; }
}
