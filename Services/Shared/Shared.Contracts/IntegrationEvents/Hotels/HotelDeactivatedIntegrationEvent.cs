namespace Shared.Contracts.IntegrationEvents.Hotels;

public record HotelDeactivatedIntegrationEvent
{
    public Guid HotelId { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
