namespace Hotels.Domain.Entities;

public class RecentActivityLog
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid? HotelId { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public string? PerformedBy { get; set; }
}
