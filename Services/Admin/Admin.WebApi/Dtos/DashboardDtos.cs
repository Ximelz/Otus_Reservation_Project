namespace Admin.WebApi.Dtos;

public record DashboardSummaryDto
{
    public int TotalHotels { get; init; }
    public int TotalRooms { get; init; }
    public int AvailableRooms { get; init; }
    public int OccupiedRooms { get; init; }
    public int ReservedRooms { get; init; }
    public int DirtyRooms { get; init; }
    public int OutOfServiceRooms { get; init; }
    public int MaintenanceRooms { get; init; }
    public decimal OccupancyRate { get; init; }
    public List<RoomTypeOccupancyDto> TopRoomTypesByOccupancy { get; init; } = [];
    public List<HotelComparisonDto> HotelComparisons { get; init; } = [];
    public List<RecentActivityItemDto> RecentActivity { get; init; } = [];
    public DateTimeOffset GeneratedAt { get; init; } = DateTimeOffset.UtcNow;
}

public record RoomTypeOccupancyDto
{
    public string RoomTypeName { get; init; } = "";
    public int TotalRooms { get; init; }
    public int OccupiedRooms { get; init; }
    public decimal OccupancyRate { get; init; }
}

public record HotelComparisonDto
{
    public Guid HotelId { get; init; }
    public string HotelName { get; init; } = "";
    public int Stars { get; init; }
    public int TotalRooms { get; init; }
    public int AvailableRooms { get; init; }
    public int OccupiedRooms { get; init; }
    public decimal OccupancyRate { get; init; }
    public int DirtyRooms { get; init; }
    public bool IsActive { get; init; }
}

public record HousekeepingBoardDto
{
    public Guid HotelId { get; init; }
    public string HotelName { get; init; } = "";
    public List<FloorGroupDto> Floors { get; init; } = [];
    public HousekeepingSummaryDto Summary { get; init; } = new();
}

public record FloorGroupDto
{
    public int Floor { get; init; }
    public List<RoomDto> Rooms { get; init; } = [];
}

public record HousekeepingSummaryDto
{
    public int TotalRooms { get; init; }
    public int CleanRooms { get; init; }
    public int DirtyRooms { get; init; }
    public int InspectedRooms { get; init; }
    public int OutOfServiceRooms { get; init; }
}

public record RecentActivityItemDto
{
    public Guid Id { get; init; }
    public Guid? HotelId { get; init; }
    public string ActivityType { get; init; } = "";
    public string Description { get; init; } = "";
    public string? EntityType { get; init; }
    public Guid? EntityId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public string? PerformedBy { get; init; }
}
