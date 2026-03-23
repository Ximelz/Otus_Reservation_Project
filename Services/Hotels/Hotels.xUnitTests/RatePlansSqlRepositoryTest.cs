using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;

namespace Hotels.xUnitTests
{
    public class RatePlansSqlRepositoryTest : IDisposable
    {
        private IRatePlansRepository _ratePlansRepository;
        private PgDbContextOptions _pgOptionsBuilder;

        public RatePlansSqlRepositoryTest()
        {
            _pgOptionsBuilder = new();
            _pgOptionsBuilder.DatabaseName = "ReservationRatePlansTest";

            using (PgDbContext dbContext = new(_pgOptionsBuilder))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            try
            {
                _ratePlansRepository = new RatePlansSqlRepository(_pgOptionsBuilder);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        public void Dispose()
        {
            using (PgDbContext dbContext = new(_pgOptionsBuilder))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task TestTypeNotExist()
        {
            IReadOnlyList<RatePlan> ratePlans = await _ratePlansRepository.Get(rp => rp.Id == Guid.Empty);
            Assert.Empty(ratePlans);
        }

        [Fact]
        public async Task TestAddRatePlan()
        {
            RatePlan newRatePlan = new();
            newRatePlan.HotelId = Guid.NewGuid();
            newRatePlan.Name = "name";
            newRatePlan.RoomTypeId = Guid.NewGuid();
            newRatePlan.BasePrice = 10.99m;

            await _ratePlansRepository.Add(newRatePlan);

            RatePlan? ratePlan = await _ratePlansRepository.Get(newRatePlan.Id);

            Assert.NotNull(ratePlan);
        }

        [Fact]
        public async Task TestRemoveRatePlan()
        {
            Guid newHotelId = Guid.NewGuid();
            Guid newRoomTypeId = Guid.NewGuid();

            Guid ratePlanId = Guid.Empty;
            for (int i = 0; i < 3; i++)
            {
                RatePlan newRatePlan = new();
                newRatePlan.HotelId = newHotelId;
                newRatePlan.Name = "name";
                newRatePlan.RoomTypeId = newRoomTypeId;
                newRatePlan.BasePrice = 10.99m;

                await _ratePlansRepository.Add(newRatePlan);

                if (i == 1)
                    ratePlanId = newRatePlan.Id;
            }

            try
            {
                await _ratePlansRepository.Remove(ratePlanId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            RatePlan? findedRatePlan = await _ratePlansRepository.Get(ratePlanId);
            Assert.Null(findedRatePlan);

            var rooms = await _ratePlansRepository.Get(rp => rp.HotelId == newHotelId);
            Assert.Equal(2, rooms.Count);

            rooms = await _ratePlansRepository.Get(rp => rp.RoomTypeId == newRoomTypeId);
            Assert.Equal(2, rooms.Count);
        }

        [Fact]
        public async Task TestUpdateRoomType()
        {
            Guid newHotelId = Guid.NewGuid();
            Guid newRoomTypeId = Guid.NewGuid();

            RatePlan? updatedRatePlan = null;

            for (int i = 0; i < 3; i++)
            {
                RatePlan newRatePlan = new();
                newRatePlan.HotelId = newHotelId;
                newRatePlan.Name = (i + 1).ToString();
                newRatePlan.BasePrice =10 + i;

                await _ratePlansRepository.Add(newRatePlan);

                if (i == 1)
                    updatedRatePlan = newRatePlan;
            }

            Assert.NotNull(updatedRatePlan);

            try
            {
                updatedRatePlan.Name = "2-1";
                await _ratePlansRepository.Update(updatedRatePlan);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            updatedRatePlan = await _ratePlansRepository.Get(updatedRatePlan.Id);
            Assert.NotNull(updatedRatePlan);
            Assert.Equal("2-1", updatedRatePlan.Name);
        }

        [Fact]
        public async Task TestGetAllByName()
        {
            Guid hotel1Id = Guid.NewGuid();
            Guid newRoomTypeId = Guid.NewGuid();

            for (int i = 0; i < 3; i++)
            {
                RatePlan newRatePlan = new();
                newRatePlan.HotelId = hotel1Id;
                newRatePlan.Name = $"name{i + 1}";
                newRatePlan.BasePrice =(i + 1) * 10;

                await _ratePlansRepository.Add(newRatePlan);
            }

            Guid hotel2Id = Guid.NewGuid();

            for (int i = 3; i < 6; i++)
            {
                RatePlan newRatePlan = new();
                newRatePlan.HotelId = hotel2Id;
                newRatePlan.Name = $"name{i + 1}";
                newRatePlan.BasePrice =(i + 1) * 10;

                await _ratePlansRepository.Add(newRatePlan);
            }

            var roomTypes = await _ratePlansRepository.Get(rp => rp.HotelId == hotel1Id && rp.Name.StartsWith("name"));
            Assert.Equal(3, roomTypes.Count);

            roomTypes = await _ratePlansRepository.Get(rp => rp.HotelId == hotel1Id && rp.Name.Contains("1"));
            Assert.Single(roomTypes);

            roomTypes = await _ratePlansRepository.Get(rp => rp.HotelId == hotel1Id && rp.Name.EndsWith("6"));
            Assert.Empty(roomTypes);

            roomTypes = await _ratePlansRepository.Get(rp => rp.HotelId == hotel1Id && rp.Name.StartsWith("Name"));
            Assert.Empty(roomTypes);
        }
    }
}
