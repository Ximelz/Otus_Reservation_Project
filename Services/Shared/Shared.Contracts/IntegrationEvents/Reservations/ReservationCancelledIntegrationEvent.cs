namespace Shared.Contracts.IntegrationEvents.Reservations;

public record ReservationCancelledIntegrationEvent
{
    public Guid ReservationId { get; init; }
    public Guid HotelId { get; init; }
    public Guid? RoomId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
