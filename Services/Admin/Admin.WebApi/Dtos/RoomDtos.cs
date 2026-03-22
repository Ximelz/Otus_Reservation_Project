using Hotels.Domain.Enums;

namespace Admin.WebApi.Dtos;

public record RoomTypeDto
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public int Capacity { get; init; }
    public int CapacityAdults { get; init; }
    public int CapacityChildren { get; init; }
    public string? BedConfiguration { get; init; }
    public decimal BaseAreaSqm { get; init; }
    public bool IsActive { get; init; }
    public int RoomCount { get; init; }
}

public record RoomTypeUpsertRequest
{
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public int Capacity { get; init; } = 2;
    public int CapacityAdults { get; init; } = 2;
    public int CapacityChildren { get; init; }
    public string? BedConfiguration { get; init; }
    public decimal BaseAreaSqm { get; init; }
}

public record RoomDto
{
    public Guid Id { get; init; }
    public Guid HotelId { get; init; }
    public string HotelName { get; init; } = "";
    public Guid TypeId { get; init; }
    public string RoomTypeName { get; init; } = "";
    public string Number { get; init; } = "";
    public int Floor { get; init; }
    public RoomStatus Status { get; init; }
    public HousekeepingStatus HousekeepingStatus { get; init; }
    public string? ViewType { get; init; }
    public string? Notes { get; init; }
    public bool IsActive { get; init; }
}

public record RoomUpsertRequest
{
    public Guid TypeId { get; init; }
    public string Number { get; init; } = "";
    public int Floor { get; init; } = 1;
    public string? ViewType { get; init; }
    public string? Notes { get; init; }
}

public record RoomStatusChangeRequest
{
    public RoomStatus Status { get; init; }
}

public record HousekeepingStatusChangeRequest
{
    public HousekeepingStatus HousekeepingStatus { get; init; }
}

public record BulkHousekeepingRequest
{
    public List<Guid> RoomIds { get; init; } = [];
    public HousekeepingStatus HousekeepingStatus { get; init; }
}
