using Hotels.Domain.Enums;

namespace Admin.WebApi.Dtos;

public record HotelListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
    public string City { get; init; } = "";
    public int Stars { get; init; }
    public bool IsActive { get; init; }
    public int TotalRooms { get; init; }
    public int AvailableRooms { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

public record HotelDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
    public int Stars { get; init; }
    public string Description { get; init; } = "";
    public string City { get; init; } = "";
    public int CountryId { get; init; }
    public string Address { get; init; } = "";
    public string Timezone { get; init; } = "";
    public string CheckInTime { get; init; } = "";
    public string CheckOutTime { get; init; } = "";
    public string Phone { get; init; } = "";
    public string Email { get; init; } = "";
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public List<HotelAmenityDto> Amenities { get; init; } = [];
    public HotelPolicyDto? Policy { get; init; }
}

public record HotelUpsertRequest
{
    public string Name { get; init; } = "";
    public string? Slug { get; init; }
    public int Stars { get; init; }
    public string Description { get; init; } = "";
    public string City { get; init; } = "";
    public int CountryId { get; init; }
    public string Address { get; init; } = "";
    public string Timezone { get; init; } = "Europe/Moscow";
    public string CheckInTime { get; init; } = "14:00";
    public string CheckOutTime { get; init; } = "12:00";
    public string Phone { get; init; } = "";
    public string Email { get; init; } = "";
}

public record HotelAmenityDto
{
    public Guid Id { get; init; }
    public AmenityType AmenityType { get; init; }
    public string? CustomName { get; init; }
    public bool IsAvailable { get; init; }
}

public record HotelAmenityUpsertRequest
{
    public AmenityType AmenityType { get; init; }
    public string? CustomName { get; init; }
    public bool IsAvailable { get; init; } = true;
}

public record HotelPolicyDto
{
    public Guid Id { get; init; }
    public string CheckInTime { get; init; } = "";
    public string CheckOutTime { get; init; } = "";
    public string? EarlyCheckInNote { get; init; }
    public string? LateCheckOutNote { get; init; }
    public string? CancellationPolicyText { get; init; }
    public bool ConfirmationPendingEnabled { get; init; }
    public bool AutoConfirmRules { get; init; }
    public string? TermsAndConditionsText { get; init; }
    public string? ContactInstructions { get; init; }
}

public record PolicyUpsertRequest
{
    public string CheckInTime { get; init; } = "14:00";
    public string CheckOutTime { get; init; } = "12:00";
    public string? EarlyCheckInNote { get; init; }
    public string? LateCheckOutNote { get; init; }
    public string? CancellationPolicyText { get; init; }
    public bool ConfirmationPendingEnabled { get; init; }
    public bool AutoConfirmRules { get; init; }
    public string? TermsAndConditionsText { get; init; }
    public string? ContactInstructions { get; init; }
}
