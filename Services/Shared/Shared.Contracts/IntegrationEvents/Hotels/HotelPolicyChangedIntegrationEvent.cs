namespace Shared.Contracts.IntegrationEvents.Hotels;

public record HotelPolicyChangedIntegrationEvent
{
    public Guid HotelId { get; init; }
    public string CheckInTime { get; init; } = string.Empty;
    public string CheckOutTime { get; init; } = string.Empty;
    public string? CancellationPolicyText { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
