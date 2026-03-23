using Hotels.Domain.Enums;

namespace Admin.WebApi.Dtos;

public record RatePlanDto
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomTypeId { get; init; }
    public string RoomTypeName { get; init; } = "";
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public decimal BasePrice { get; init; }
    public string Currency { get; init; } = "RUB";
    public CancellationPolicyType CancellationPolicyType { get; init; }
    public bool BreakfastIncluded { get; init; }
    public bool PrepaymentRequired { get; init; }
    public bool IsDefault { get; init; }
    public bool IsActive { get; init; }
}

public record RatePlanUpsertRequest
{
    public Guid RoomTypeId { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public decimal BasePrice { get; init; }
    public string Currency { get; init; } = "RUB";
    public CancellationPolicyType CancellationPolicyType { get; init; }
    public bool BreakfastIncluded { get; init; }
    public bool PrepaymentRequired { get; init; }
    public bool IsDefault { get; init; }
}
