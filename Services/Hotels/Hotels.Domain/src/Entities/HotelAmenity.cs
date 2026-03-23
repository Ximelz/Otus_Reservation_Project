using Hotels.Domain.Enums;

namespace Hotels.Domain.Entities;

public class HotelAmenity
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid HotelId { get; set; } = Guid.Empty;
    public AmenityType AmenityType { get; set; }
    public string? CustomName { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Hotel? Hotel { get; set; }
}
