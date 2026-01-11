using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;

namespace Hotels.xUnitTests
{
    public class RoomTypesSqlRepositoryTest : IDisposable
    {
        private IRoomTypesRepository _roomTypesRepository;
        private IHotelsRepository _hotelsRepository;
        private PgDbContextOptions _pgOptionsBuilder;

        public RoomTypesSqlRepositoryTest() : base()
        {
            _pgOptionsBuilder = new();
            _pgOptionsBuilder.DatabaseName = "ReservationRoomTypesTest";

            using (PgDbContext dbContext = new(_pgOptionsBuilder))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            try
            {
                _roomTypesRepository = new RoomTypesSqlRepository(_pgOptionsBuilder);
                _hotelsRepository = new HotelsSqlRepository(_pgOptionsBuilder);
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
            RoomType? roomType = await _roomTypesRepository.Get(-1);

            Assert.True(roomType == null);
        }

        [Fact]
        public async Task TestAddRoomType()
        {
            RoomType newRoomType = new();
            newRoomType.HotelId = Guid.NewGuid();
            newRoomType.Name = "name";
            newRoomType.Description = newRoomType.Name;

            await _roomTypesRepository.Add(newRoomType);

            RoomType? roomType = await _roomTypesRepository.Get(newRoomType.Id);

            Assert.NotNull(roomType);
        }

        [Fact]
        public async Task TestGetAllByHotel()
        { 
            Guid hotel1Id = Guid.NewGuid();

            for (int i = 0; i < 3; i++)
            {
                RoomType newRoomType = new();
                newRoomType.HotelId = hotel1Id;
                newRoomType.Name = (i + 1).ToString();
                newRoomType.Description = newRoomType.Name;

                await _roomTypesRepository.Add(newRoomType);
            }

            RoomType newRoomType2 = new();
            newRoomType2.HotelId = Guid.NewGuid();
            newRoomType2.Name = "4";
            newRoomType2.Description = newRoomType2.Name;

            await _roomTypesRepository.Add(newRoomType2);

            var roomTypes = await _roomTypesRepository.GetAllByHotel(hotel1Id);
            Assert.Equal(3, roomTypes.Count);
        }

        [Fact]
        public async Task TestRemoveRoomType()
        {
            Guid newHotelId = Guid.NewGuid();

            long roomTypeId = -1;
            for (int i = 0; i < 3; i++)
            {
                RoomType newRoomType = new();
                newRoomType.HotelId = newHotelId;
                newRoomType.Name = (i + 1).ToString();
                newRoomType.Description = newRoomType.Name;

                await _roomTypesRepository.Add(newRoomType);

                if (i == 1)
                    roomTypeId = newRoomType.Id;
            }

            try
            {
                await _roomTypesRepository.Remove(roomTypeId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            RoomType? findedRoomType = await _roomTypesRepository.Get(roomTypeId);
            Assert.Null(findedRoomType);

            var rooms = await _roomTypesRepository.GetAllByHotel(newHotelId);
            Assert.Equal(2, rooms.Count);
        }

        [Fact]
        public async Task TestUpdateRoomType()
        {
            Guid newHotelId = Guid.NewGuid();

            RoomType? updatedRoomType = null;

            for (int i = 0; i < 3; i++)
            {
                RoomType newRoomType = new();
                newRoomType.HotelId = newHotelId;
                newRoomType.Name = (i + 1).ToString();
                newRoomType.Description = newRoomType.Name;

                await _roomTypesRepository.Add(newRoomType);

                if (i == 1)
                    updatedRoomType = newRoomType;
            }

            Assert.NotNull(updatedRoomType);

            try
            {
                updatedRoomType.Name = "2-1";
                await _roomTypesRepository.Update(updatedRoomType);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            updatedRoomType = await _roomTypesRepository.Get(updatedRoomType.Id);
            Assert.NotNull(updatedRoomType);
            Assert.Equal("2-1", updatedRoomType.Name);
        }

        [Fact]
        public async Task TestGetAllByName()
        {
            Guid hotel1Id = Guid.NewGuid();

            for (int i = 0; i < 3; i++)
            {
                RoomType newRoomType = new();
                newRoomType.HotelId = hotel1Id;
                newRoomType.Name = $"name{i + 1}";
                newRoomType.Description = newRoomType.Name;

                await _roomTypesRepository.Add(newRoomType);
            }

            Guid hotel2Id = Guid.NewGuid();

            for (int i = 3; i < 6; i++)
            {
                RoomType newRoomType = new();
                newRoomType.HotelId = hotel2Id;
                newRoomType.Name = $"name{i + 1}";
                newRoomType.Description = newRoomType.Name;

                await _roomTypesRepository.Add(newRoomType);
            }

            var roomTypes = await _roomTypesRepository.GetAllByName(hotel1Id, "name");
            Assert.Equal(3, roomTypes.Count);

            roomTypes = await _roomTypesRepository.GetAllByName(hotel1Id, "1");
            Assert.Single(roomTypes);

            roomTypes = await _roomTypesRepository.GetAllByName(hotel1Id, "6");
            Assert.Empty(roomTypes);

            roomTypes = await _roomTypesRepository.GetAllByName(hotel1Id, "Name");
            Assert.Empty(roomTypes);
        }
    }
}
