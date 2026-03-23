using Hotels.Domain.Enums;

namespace Hotels.Domain.Entities;

public class Room
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid HotelId { get; set; } = Guid.Empty;
    public string Number { get; set; } = string.Empty;
    public Guid TypeId { get; set; } = Guid.Empty;
    public int Floor { get; set; } = 1;
    public RoomStatus Status { get; set; } = RoomStatus.Available;
    public HousekeepingStatus HousekeepingStatus { get; set; } = HousekeepingStatus.Clean;
    public string? ViewType { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Hotel? Hotel { get; set; }
    public RoomType? RoomType { get; set; }
}
