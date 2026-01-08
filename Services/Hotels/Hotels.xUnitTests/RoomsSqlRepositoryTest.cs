using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;

namespace Hotels.xUnitTests
{
    public class RoomsSqlRepositoryTest : IDisposable
    {
        private IRoomsRepository _roomsRepository;
        private IHotelsRepository _hotelsRepository;
        private PgDbContextOptions _pgOptionsBuilder;

        public RoomsSqlRepositoryTest() : base()
        {
            _pgOptionsBuilder = new();
            _pgOptionsBuilder.DatabaseName = "ReservationRoomsTest";

            using (SqlDatabaseContext dbContext = new(_pgOptionsBuilder.GetOptions()))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            try
            {
                _roomsRepository = new RoomsSqlRepository(_pgOptionsBuilder.GetOptions());
                _hotelsRepository = new HotelsSqlRepository(_pgOptionsBuilder.GetOptions());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        public void Dispose()
        {
            using (SqlDatabaseContext dbContext = new(_pgOptionsBuilder.GetOptions()))
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

            for (int i = 0; i < 3; i++)
            {
                Room newRoom = new();
                newRoom.HotelId = newHotel.Id;
                newRoom.Number = (i + 1).ToString();
                newRoom.Capacity = 2;

                await _roomsRepository.Add(newRoom);
            }

            var rooms = await _roomsRepository.GetAllByHotel(newHotel.Id);
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
                newRoom.Capacity = 2;

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

            Room? findedRoom = await _roomsRepository.Get(roomId);
            Assert.Null(findedRoom);

            var rooms = await _roomsRepository.GetAllByHotel(newHotel.Id);
            Assert.Equal(2, rooms.Count);
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
                newRoom.Capacity = 2;

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
