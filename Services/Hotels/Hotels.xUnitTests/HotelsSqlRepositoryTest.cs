using Hotels.Infrastructure;
using Hotels.Domain.Entities;
using Hotels.Infrastructure.Repositories;

namespace Hotels.xUnitTests
{
    public class HotelsSqlRepositoryTest
    {
        private string _dbConfigurationFilePath = "./DbConfiguration.json";
        private HotelsSqlRepository _hotelsRepository;

        public HotelsSqlRepositoryTest()
        {
            Assert.True(File.Exists(_dbConfigurationFilePath));

            PgDbContextOptions pgOptionsBuilder = new(_dbConfigurationFilePath);

            try
            {
                _hotelsRepository = new HotelsSqlRepository(pgOptionsBuilder.GetOptions());

                using (SqlDatabaseContext dbContext = new(pgOptionsBuilder.GetOptions()))
                {
                    dbContext.Database.EnsureDeleted();
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task TestHotelNotExist()
        {
            Hotel? hotel = await _hotelsRepository.Get(-1);

            Assert.True(hotel == null);
        }

        [Fact]
        public async Task TestRemoveHotel()
        {
            Hotel newHotel = new();

            // ставим уникальное имя, по которому будем искать добавленную запись
            newHotel.Name = "Astoria";
            newHotel.Stars = 5;
            newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel.CountryId = 1;
            newHotel.Phone = "+74924123377";
            newHotel.Email = "service@gh-astoria-hotel.ru";

            // добавление записи отеля
            try
            {
                await _hotelsRepository.Add(newHotel);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Hotel? findedHotel = await _hotelsRepository.Get(newHotel.Id);
            Assert.NotNull(findedHotel);

            //---------------------------------------------------------------------

            Hotel newHotel2 = new();

            // ставим уникальное имя, по которому будем искать добавленную запись
            newHotel2.Name = "Astoria";
            newHotel2.Stars = 5;
            newHotel2.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel2.CountryId = 1;
            newHotel2.Phone = "+74924123377";
            newHotel2.Email = "service@gh-astoria-hotel.ru";

            // добавление записи отеля
            try
            {
                await _hotelsRepository.Add(newHotel2);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Hotel? findedHotel2 = await _hotelsRepository.Get(newHotel2.Id);
            Assert.NotNull(findedHotel2);

            try
            {
                await _hotelsRepository.Remove(newHotel.Id);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Hotel? removedHotel = await _hotelsRepository.Get(newHotel.Id);
            Assert.True(removedHotel == null);

            // проверить то, что осталась вторая запись, то есть что удалена только одна запись
            findedHotel2 = await _hotelsRepository.Get(newHotel2.Id);
            Assert.NotNull(findedHotel2);
        }

        [Fact]
        public async Task TestHotelAdd()
        {
            Hotel newHotel = new();

            // ставим уникальное имя, по которому будем искать добавленную запись
            newHotel.Name = "Astoria";
            newHotel.Stars = 5;
            newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel.CountryId = 1;
            newHotel.Phone = "+74924123377";
            newHotel.Email = "service@gh-astoria-hotel.ru";

            long id = 0;
            // добавление записи отеля
            try
            {
                id = await _hotelsRepository.Add(newHotel);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Equal(id, newHotel.Id);

            Hotel? findedHotel = await _hotelsRepository.Get(newHotel.Id);
            Assert.NotNull(findedHotel);

            await _hotelsRepository.Remove(id);

            findedHotel = await _hotelsRepository.Get(newHotel.Id);
            Assert.True(findedHotel == null);
        }

        [Fact]
        public async Task TestHotelUpdate()
        {
            Hotel newHotel = new();

            // ставим уникальное имя, по которому будем искать добавленную запись
            newHotel.Name = "Astoria";
            newHotel.Stars = 5;
            newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel.CountryId = 1;
            newHotel.Phone = "+74924123377";
            newHotel.Email = "service@gh-astoria-hotel.ru";

            // добавление записи отеля
            try
            {
                await _hotelsRepository.Add(newHotel);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Hotel? findedHotel = await _hotelsRepository.Get(newHotel.Id);
            Assert.NotNull(findedHotel);

            // проверка обновления отеля
            newHotel.Name = "Astoria Pro";

            try
            {
                await _hotelsRepository.Update(newHotel);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            findedHotel = await _hotelsRepository.Get(newHotel.Id);
            Assert.NotNull(findedHotel);

            Assert.Equal("Astoria Pro", findedHotel.Name);

            await _hotelsRepository.Remove(newHotel.Id);
        }

        [Fact]
        public async Task TestHotelGetByCountry()
        {
            Hotel newHotel = new();

            // ставим уникальное имя, по которому будем искать добавленную запись
            newHotel.Name = "Astoria";
            newHotel.Stars = 5;
            newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel.CountryId = 1;
            newHotel.Phone = "+74924123377";
            newHotel.Email = "service@gh-astoria-hotel.ru";

            // добавление записи отеля
            try
            {
                await _hotelsRepository.Add(newHotel);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // поиск отеля по стране, в которой находится
            IReadOnlyList<Hotel> hotels = new List<Hotel>();
            try
            {
                hotels = await _hotelsRepository.GetAllByCountry(1);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            hotels.Where(h => h.Name == newHotel.Name).ToList();

            Assert.True(hotels.Count == 1);
            Assert.Equal(newHotel.Id, hotels.First().Id);

            await _hotelsRepository.Remove(newHotel.Id);
        }

        [Fact]
        public async Task TestHotelGetByStars()
        {
            Hotel newHotel = new();

            // ставим уникальное имя, по которому будем искать добавленную запись
            newHotel.Name = "Astoria";
            newHotel.Stars = 5;
            newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel.CountryId = 1;
            newHotel.Phone = "+74924123377";
            newHotel.Email = "service@gh-astoria-hotel.ru";

            // добавление записи отеля
            try
            {
                await _hotelsRepository.Add(newHotel);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            IReadOnlyList<Hotel> hotels = new List<Hotel>();

            // поиск отеля по звездам, к которым не относится
            try
            {
                hotels = await _hotelsRepository.GetAllByStars([1]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            hotels.Where(h => h.Name == newHotel.Name).ToList();

            Assert.True(hotels.Count == 0);

            // поиск отеля по звездам, к которым относится
            try
            {
                hotels = await _hotelsRepository.GetAllByStars([5]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            hotels.Where(h => h.Name == newHotel.Name).ToList();

            Assert.True(hotels.Count == 1);

            var addedHotel = hotels.First();
            Assert.Equal(newHotel.Name, addedHotel.Name);

            await _hotelsRepository.Remove(newHotel.Id);
        }
    }
}
