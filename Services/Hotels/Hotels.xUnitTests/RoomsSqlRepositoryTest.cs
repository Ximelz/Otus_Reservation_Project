using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;

namespace Hotels.xUnitTests
{
    public class RoomsSqlRepositoryTest : IDisposable
    {
        private IRoomsRepository _roomsRepository;
        private IRoomTypesRepository _roomTypesRepository;
        private IHotelsRepository _hotelsRepository;
        private PgDbContextOptions _pgOptionsBuilder;

        public RoomsSqlRepositoryTest() : base()
        {
            _pgOptionsBuilder = new();
            _pgOptionsBuilder.DatabaseName = "ReservationRoomsTest";

            using (PgDbContext dbContext = new(_pgOptionsBuilder))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            try
            {
                _roomsRepository = new RoomsSqlRepository(_pgOptionsBuilder);
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
        public async Task TestHotelNotExist()
        {
            Room? room = await _roomsRepository.Get(Guid.Empty);

            Assert.True(room == null);
        }

        [Fact]
        public async Task TestAddRoom()
        {
            Hotel? newHotel = await CreateHotel();

            RoomType roomType = new();
            roomType.HotelId = newHotel.Id;
            roomType.Name = "test";
            roomType.Description = "test description";
            roomType.Capacity = 1;

            await _roomTypesRepository.Add(roomType);

            for (int i = 0; i < 3; i++)
            {
                Room newRoom = new();
                newRoom.HotelId = newHotel.Id;
                newRoom.TypeId = roomType.Id;
                newRoom.Number = (i + 1).ToString();

                await _roomsRepository.Add(newRoom);
            }

            var rooms = await _roomsRepository.GetAllByParameters(newHotel.Id);
            Assert.Equal(3, rooms.Count);
        }

        [Fact]
        public async Task TestRemoveRoom()
        {
            Hotel newHotel = await CreateHotel();

            Guid roomId = Guid.Empty;
            for (int i = 0; i < 3; i++)
            {
                Room newRoom = new();
                newRoom.HotelId = newHotel.Id;
                newRoom.Number = (i+1).ToString();

                await _roomsRepository.Add(newRoom);

                if (i == 1)
                    roomId = newRoom.Id;
            }

            try
            {
                await _roomsRepository.Remove(roomId);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Room? foundedRoom = await _roomsRepository.Get(roomId);
            Assert.Null(foundedRoom);
            if (foundedRoom == null)
                return;

            var rooms = await _roomsRepository.GetAllByParameters(newHotel.Id);
            Assert.Empty(rooms);

            RoomType roomType = new();
            roomType.HotelId = newHotel.Id;
            roomType.Name = "test";
            roomType.Description = "test description";
            roomType.Capacity = 1;

            await _roomTypesRepository.Add(roomType);
            foundedRoom.TypeId = roomType.Id;
            await _roomsRepository.Update(foundedRoom);

            rooms = await _roomsRepository.GetAllByParameters(newHotel.Id);
            Assert.Single(rooms);
        }

        [Fact]
        public async Task TestUpdateRoom()
        {
            Hotel newHotel = await CreateHotel();

            Room? updatedRoom = null;

            for (int i = 0; i < 3; i++)
            {
                Room newRoom = new();
                newRoom.HotelId = newHotel.Id;
                newRoom.Number = (i + 1).ToString();

                await _roomsRepository.Add(newRoom);

                if (i == 1)
                    updatedRoom = newRoom;
            }

            Assert.NotNull(updatedRoom);

            try
            {
                updatedRoom.Number = "2-1";
                await _roomsRepository.Update(updatedRoom);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            updatedRoom = await _roomsRepository.Get(updatedRoom.Id);
            Assert.NotNull(updatedRoom);
            Assert.Equal("2-1", updatedRoom.Number);
        }

        [Fact]
        public async Task TestGetRoomsByCapacity()
        {
            Hotel? newHotel = await CreateHotel();

            Dictionary<int, RoomType> roomTypes = new();
            for (int c = 1; c < 4; c++)
            {
                RoomType roomType = new();
                roomType.HotelId = newHotel.Id;
                roomType.Name = "test";
                roomType.Description = "test description";
                roomType.Capacity = c;

                roomTypes[c] = roomType;
                await _roomTypesRepository.Add(roomType);

                for (int i = 0; i < 3; i++)
                {
                    Room newRoom = new();
                    newRoom.HotelId = newHotel.Id;
                    newRoom.TypeId = roomType.Id;
                    newRoom.Number = (i + 1).ToString();

                    await _roomsRepository.Add(newRoom);
                }
            }

            var rooms = await _roomsRepository.GetAllByParameters(newHotel.Id, capacity: 1);
            Assert.Equal(3, rooms.Count);

            rooms = await _roomsRepository.GetAllByParameters(newHotel.Id, capacity: 2);
            Assert.Equal(3, rooms.Count);

            rooms = await _roomsRepository.GetAllByParameters(newHotel.Id, capacity: 3);
            Assert.Equal(3, rooms.Count);

            rooms = await _roomsRepository.GetAllByParameters(newHotel.Id, capacity: 4);
            Assert.Empty(rooms);

            rooms = await _roomsRepository.GetAllByParameters(newHotel.Id, capacity: 0);
            Assert.Equal(9, rooms.Count);
        }

        private async Task<Hotel> CreateHotel()
        {
            Hotel newHotel = new Hotel();
            newHotel.Name = "New Hotel";
            newHotel.Stars = 5;
            newHotel.Address = "City, Street";
            newHotel.CountryId = 1;
            newHotel.Phone = "+7987654321";
            newHotel.Email = "service@hotel.ru";

            await _hotelsRepository.Add(newHotel);

            return newHotel;
        }
    }
}
