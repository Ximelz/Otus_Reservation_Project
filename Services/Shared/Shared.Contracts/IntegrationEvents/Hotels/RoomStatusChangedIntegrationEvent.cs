using Shared.Contracts.Enums;

namespace Shared.Contracts.IntegrationEvents.Hotels;

public record RoomStatusChangedIntegrationEvent
{
    public Guid RoomId { get; init; }
    public Guid HotelId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public RoomStatus PreviousStatus { get; init; }
    public RoomStatus NewStatus { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
