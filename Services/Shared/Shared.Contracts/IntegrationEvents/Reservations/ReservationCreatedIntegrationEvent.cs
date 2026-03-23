namespace Shared.Contracts.IntegrationEvents.Reservations;

public record ReservationCreatedIntegrationEvent
{
    public Guid ReservationId { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomTypeId { get; init; }
    public Guid? RoomId { get; init; }
    public string GuestName { get; init; } = string.Empty;
    public DateOnly CheckInDate { get; init; }
    public DateOnly CheckOutDate { get; init; }
    public int Adults { get; init; }
    public int Children { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
