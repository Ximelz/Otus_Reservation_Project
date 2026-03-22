namespace Shared.Contracts.IntegrationEvents.Reservations;

public record ReservationRoomAssignedIntegrationEvent
{
    public Guid ReservationId { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public DateOnly CheckInDate { get; init; }
    public DateOnly CheckOutDate { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
