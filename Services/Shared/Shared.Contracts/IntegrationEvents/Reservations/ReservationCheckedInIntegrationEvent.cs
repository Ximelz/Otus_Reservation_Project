namespace Shared.Contracts.IntegrationEvents.Reservations;

public record ReservationCheckedInIntegrationEvent
{
    public Guid ReservationId { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public string GuestName { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
