namespace Hotels.Domain.Entities;

public class Hotel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public int Stars { get; set; }
    public string Description { get; set; } = "";
    public string City { get; set; } = "";
    public int CountryId { get; set; }
    public string Address { get; set; } = "";
    public string Timezone { get; set; } = "Europe/Moscow";
    public TimeOnly CheckInTime { get; set; } = new(14, 0);
    public TimeOnly CheckOutTime { get; set; } = new(12, 0);
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public List<HotelAmenity> Amenities { get; set; } = [];
    public List<RoomType> RoomTypes { get; set; } = [];
    public List<Room> Rooms { get; set; } = [];
    public List<RatePlan> RatePlans { get; set; } = [];
    public HotelPolicy? Policy { get; set; }
}
