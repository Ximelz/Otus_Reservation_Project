using Hotels.Domain.Enums;

namespace Hotels.Domain.Entities;

public class RatePlan
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid HotelId { get; set; } = Guid.Empty;
    public Guid RoomTypeId { get; set; } = Guid.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Currency { get; set; } = "RUB";
    public CancellationPolicyType CancellationPolicyType { get; set; } = CancellationPolicyType.Free;
    public bool BreakfastIncluded { get; set; }
    public bool PrepaymentRequired { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Hotel? Hotel { get; set; }
    public RoomType? RoomType { get; set; }
}
