using Hotels.Domain.Entities;
using Hotels.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hotels.Infrastructure.Seed;

public class HotelsSeedService : IHostedService
{
    private readonly PgDbContextOptions _pgOptions;
    private readonly ILogger<HotelsSeedService> _logger;

    public HotelsSeedService(PgDbContextOptions pgOptions, ILogger<HotelsSeedService> logger)
    {
        _pgOptions = pgOptions;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        using var db = new PgDbContext(_pgOptions);
        //await db.Database.MigrateAsync(ct);

        if (await db.Hotels.AnyAsync(ct))
        {
            _logger.LogInformation("Seed data already exists, skipping");
            return;
        }

        _logger.LogInformation("Seeding demo data...");

        // Country 643 (Russia) is already seeded by the Initial migration
        // Skip adding it again to avoid duplicate key error

        var hotels = CreateHotels();
        db.Hotels.AddRange(hotels);

        foreach (var hotel in hotels)
        {
            var roomTypes = CreateRoomTypes(hotel.Id);
            db.RoomTypes.AddRange(roomTypes);

            var rooms = CreateRooms(hotel.Id, roomTypes);
            db.Rooms.AddRange(rooms);

            var ratePlans = CreateRatePlans(hotel.Id, roomTypes);
            db.RatePlans.AddRange(ratePlans);

            var amenities = CreateAmenities(hotel.Id);
            db.HotelAmenities.AddRange(amenities);

            db.HotelPolicies.Add(CreatePolicy(hotel.Id));
        }

        var activityLogs = CreateActivityLogs(hotels);
        db.RecentActivityLogs.AddRange(activityLogs);

        await db.SaveChangesAsync(ct);
        _logger.LogInformation("Demo data seeded: {Count} hotels", hotels.Count);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    private static List<Hotel> CreateHotels() =>
    [
        new()
        {
            Id = Guid.Parse("a1000000-0000-0000-0000-000000000001"),
            Name = "Grand Aurora Hotel",
            Slug = "grand-aurora-hotel",
            Stars = 5, City = "Москва", CountryId = 643,
            Address = "ул. Тверская, 22", Timezone = "Europe/Moscow",
            CheckInTime = new TimeOnly(14, 0), CheckOutTime = new TimeOnly(12, 0),
            Phone = "+7 (495) 123-45-67", Email = "info@grand-aurora.ru",
            Description = "Роскошный пятизвёздочный отель в самом центре Москвы с видом на Кремль",
            IsActive = true
        },
        new()
        {
            Id = Guid.Parse("a2000000-0000-0000-0000-000000000002"),
            Name = "Riverside Business Hotel",
            Slug = "riverside-business-hotel",
            Stars = 4, City = "Санкт-Петербург", CountryId = 643,
            Address = "Невский проспект, 88", Timezone = "Europe/Moscow",
            CheckInTime = new TimeOnly(15, 0), CheckOutTime = new TimeOnly(11, 0),
            Phone = "+7 (812) 987-65-43", Email = "booking@riverside-biz.ru",
            Description = "Современный бизнес-отель на набережной Невы",
            IsActive = true
        },
        new()
        {
            Id = Guid.Parse("a3000000-0000-0000-0000-000000000003"),
            Name = "Old Town Boutique Suites",
            Slug = "old-town-boutique-suites",
            Stars = 3, City = "Казань", CountryId = 643,
            Address = "ул. Баумана, 15", Timezone = "Europe/Moscow",
            CheckInTime = new TimeOnly(14, 0), CheckOutTime = new TimeOnly(12, 0),
            Phone = "+7 (843) 555-12-34", Email = "hello@oldtown-suites.ru",
            Description = "Уютный бутик-отель в историческом центре Казани",
            IsActive = true
        }
    ];

    private static List<RoomType> CreateRoomTypes(Guid hotelId)
    {
        var prefix = hotelId.ToString()[..2];
        return
        [
            new() { Id = Guid.NewGuid(), HotelId = hotelId, Code = "STD", Name = "Standard", Description = "Стандартный номер", Capacity = 2, CapacityAdults = 2, CapacityChildren = 1, BedConfiguration = "1 Double", BaseAreaSqm = 22, IsActive = true },
            new() { Id = Guid.NewGuid(), HotelId = hotelId, Code = "SUP", Name = "Superior", Description = "Улучшенный номер", Capacity = 2, CapacityAdults = 2, CapacityChildren = 1, BedConfiguration = "1 King", BaseAreaSqm = 30, IsActive = true },
            new() { Id = Guid.NewGuid(), HotelId = hotelId, Code = "DLX", Name = "Deluxe", Description = "Номер Делюкс", Capacity = 2, CapacityAdults = 2, CapacityChildren = 2, BedConfiguration = "1 King + Sofa", BaseAreaSqm = 40, IsActive = true },
            new() { Id = Guid.NewGuid(), HotelId = hotelId, Code = "FAM", Name = "Family Suite", Description = "Семейный номер", Capacity = 4, CapacityAdults = 2, CapacityChildren = 2, BedConfiguration = "2 Double", BaseAreaSqm = 55, IsActive = true },
            new() { Id = Guid.NewGuid(), HotelId = hotelId, Code = "EXE", Name = "Executive Suite", Description = "Представительский люкс", Capacity = 2, CapacityAdults = 2, CapacityChildren = 0, BedConfiguration = "1 King + Office", BaseAreaSqm = 65, IsActive = true },
        ];
    }

    private static List<Room> CreateRooms(Guid hotelId, List<RoomType> roomTypes)
    {
        var rooms = new List<Room>();
        var random = new Random(hotelId.GetHashCode());
        var statuses = new[] { RoomStatus.Available, RoomStatus.Occupied, RoomStatus.Reserved, RoomStatus.Available, RoomStatus.Available };
        var hkStatuses = new[] { HousekeepingStatus.Clean, HousekeepingStatus.Dirty, HousekeepingStatus.Inspected, HousekeepingStatus.Clean };
        var views = new[] { "City", "Garden", "Pool", "Street", null };

        for (int floor = 1; floor <= 4; floor++)
        {
            for (int i = 1; i <= 4; i++)
            {
                var roomNum = $"{floor}{i:D2}";
                var typeIdx = (floor + i) % roomTypes.Count;
                var status = statuses[random.Next(statuses.Length)];
                var hk = status == RoomStatus.Occupied ? HousekeepingStatus.Dirty : hkStatuses[random.Next(hkStatuses.Length)];
                var isOos = floor == 3 && i == 4;

                rooms.Add(new Room
                {
                    Id = Guid.NewGuid(), HotelId = hotelId, TypeId = roomTypes[typeIdx].Id,
                    Number = roomNum, Floor = floor,
                    Status = isOos ? RoomStatus.OutOfService : status,
                    HousekeepingStatus = isOos ? HousekeepingStatus.Dirty : hk,
                    ViewType = views[random.Next(views.Length)],
                    IsActive = !isOos
                });
            }
        }
        return rooms;
    }

    private static List<RatePlan> CreateRatePlans(Guid hotelId, List<RoomType> roomTypes)
    {
        var plans = new List<RatePlan>();
        decimal[] basePrices = [4500, 6500, 9000, 12000, 18000];

        for (int i = 0; i < roomTypes.Count; i++)
        {
            var rt = roomTypes[i];
            var price = basePrices[i];

            plans.Add(new RatePlan
            {
                Id = Guid.NewGuid(), HotelId = hotelId, RoomTypeId = rt.Id,
                Code = $"{rt.Code}-STD", Name = $"{rt.Name} - Standard Rate",
                BasePrice = price, Currency = "RUB",
                CancellationPolicyType = CancellationPolicyType.Free,
                BreakfastIncluded = false, PrepaymentRequired = false,
                IsDefault = true, IsActive = true
            });

            plans.Add(new RatePlan
            {
                Id = Guid.NewGuid(), HotelId = hotelId, RoomTypeId = rt.Id,
                Code = $"{rt.Code}-BRK", Name = $"{rt.Name} - With Breakfast",
                BasePrice = price + 1200, Currency = "RUB",
                CancellationPolicyType = CancellationPolicyType.Moderate,
                BreakfastIncluded = true, PrepaymentRequired = false,
                IsDefault = false, IsActive = true
            });

            plans.Add(new RatePlan
            {
                Id = Guid.NewGuid(), HotelId = hotelId, RoomTypeId = rt.Id,
                Code = $"{rt.Code}-NRF", Name = $"{rt.Name} - Non-Refundable",
                BasePrice = price * 0.8m, Currency = "RUB",
                CancellationPolicyType = CancellationPolicyType.NonRefundable,
                BreakfastIncluded = false, PrepaymentRequired = true,
                IsDefault = false, IsActive = true
            });
        }
        return plans;
    }

    private static List<HotelAmenity> CreateAmenities(Guid hotelId) =>
    [
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.WiFi, IsAvailable = true },
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.Parking, IsAvailable = true },
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.Breakfast, IsAvailable = true },
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.Gym, IsAvailable = true },
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.Spa, IsAvailable = hotelId.ToString().StartsWith("a1") },
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.Transfer, IsAvailable = true },
        new() { Id = Guid.NewGuid(), HotelId = hotelId, AmenityType = AmenityType.ConferenceRoom, IsAvailable = true },
    ];

    private static HotelPolicy CreatePolicy(Guid hotelId) => new()
    {
        Id = Guid.NewGuid(), HotelId = hotelId,
        CheckInTime = new TimeOnly(14, 0), CheckOutTime = new TimeOnly(12, 0),
        EarlyCheckInNote = "Ранний заезд возможен при наличии свободных номеров (доплата 50%)",
        LateCheckOutNote = "Поздний выезд до 18:00 - 50% от стоимости, после 18:00 - полная стоимость",
        CancellationPolicyText = "Бесплатная отмена за 24 часа. При поздней отмене взимается стоимость первой ночи.",
        ConfirmationPendingEnabled = true, AutoConfirmRules = false,
        TermsAndConditionsText = "Гости обязаны предъявить документ, удостоверяющий личность, при заезде.",
        ContactInstructions = "Для связи с рецепцией наберите 0 с телефона в номере"
    };

    private static List<RecentActivityLog> CreateActivityLogs(List<Hotel> hotels)
    {
        var logs = new List<RecentActivityLog>();
        var now = DateTimeOffset.UtcNow;

        foreach (var hotel in hotels)
        {
            logs.Add(new RecentActivityLog
            {
                Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "HotelCreated",
                Description = $"Hotel '{hotel.Name}' created", EntityType = "Hotel",
                EntityId = hotel.Id, Timestamp = now.AddDays(-30), PerformedBy = "admin@demo.local"
            });
            logs.Add(new RecentActivityLog
            {
                Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "RoomStatusChanged",
                Description = "Room 201: Available → Occupied (guest check-in)", EntityType = "Room",
                Timestamp = now.AddHours(-6), PerformedBy = "admin@demo.local"
            });
            logs.Add(new RecentActivityLog
            {
                Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "HousekeepingChanged",
                Description = "Room 103: Dirty → Clean (housekeeping completed)", EntityType = "Room",
                Timestamp = now.AddHours(-3), PerformedBy = "admin@demo.local"
            });
            logs.Add(new RecentActivityLog
            {
                Id = Guid.NewGuid(), HotelId = hotel.Id, ActivityType = "RatePlanCreated",
                Description = "New rate plan 'Summer Special' added", EntityType = "RatePlan",
                Timestamp = now.AddDays(-7), PerformedBy = "admin@demo.local"
            });
        }
        return logs;
    }
}
