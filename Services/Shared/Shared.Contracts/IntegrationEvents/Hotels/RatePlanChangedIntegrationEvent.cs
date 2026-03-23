using Shared.Contracts.Enums;

namespace Shared.Contracts.IntegrationEvents.Hotels;

public record RatePlanChangedIntegrationEvent
{
    public Guid RatePlanId { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomTypeId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public string Currency { get; init; } = "RUB";
    public CancellationPolicyType CancellationPolicy { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}
