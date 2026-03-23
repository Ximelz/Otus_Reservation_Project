namespace Shared.Contracts.IntegrationEvents.Hotels;

public record HotelUpdatedIntegrationEvent
{
    public Guid HotelId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public int Stars { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
