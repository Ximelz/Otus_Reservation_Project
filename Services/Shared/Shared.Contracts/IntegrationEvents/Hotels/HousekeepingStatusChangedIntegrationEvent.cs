using Shared.Contracts.Enums;

namespace Shared.Contracts.IntegrationEvents.Hotels;

public record HousekeepingStatusChangedIntegrationEvent
{
    public Guid RoomId { get; init; }
    public Guid HotelId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public string HotelName { get; init; } = string.Empty;
    public HousekeepingStatus PreviousStatus { get; init; }
    public HousekeepingStatus NewStatus { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
