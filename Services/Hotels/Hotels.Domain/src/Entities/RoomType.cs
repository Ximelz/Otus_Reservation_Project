namespace Hotels.Domain.Entities;

public class RoomType
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid HotelId { get; set; } = Guid.Empty;
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; } = 1;
    public int CapacityAdults { get; set; } = 2;
    public int CapacityChildren { get; set; }
    public string? BedConfiguration { get; set; }
    public decimal BaseAreaSqm { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Hotel? Hotel { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public List<RatePlan> RatePlans { get; set; } = [];
}
