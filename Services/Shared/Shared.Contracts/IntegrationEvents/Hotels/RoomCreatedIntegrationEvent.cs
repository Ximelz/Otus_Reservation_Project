namespace Shared.Contracts.IntegrationEvents.Hotels;

public record RoomCreatedIntegrationEvent
{
    public Guid RoomId { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomTypeId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public int Floor { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
