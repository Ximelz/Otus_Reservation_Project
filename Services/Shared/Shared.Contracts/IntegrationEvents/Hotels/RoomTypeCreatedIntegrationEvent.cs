namespace Shared.Contracts.IntegrationEvents.Hotels;

public record RoomTypeCreatedIntegrationEvent
{
    public Guid RoomTypeId { get; init; }
    public Guid HotelId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int CapacityAdults { get; init; }
    public int CapacityChildren { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
