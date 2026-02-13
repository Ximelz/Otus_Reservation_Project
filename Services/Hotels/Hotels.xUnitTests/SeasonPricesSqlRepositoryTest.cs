using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Domain.Services;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;
using Hotels.Infrastructure.Services;

namespace Hotels.xUnitTests
{
    public class SeasonPricesSqlRepositoryTest : IDisposable
    {
        private ISeasonPriceRepository _seasonPriceRepository;
        private PgDbContextOptions _pgOptionsBuilder;

        public SeasonPricesSqlRepositoryTest() : base()
        {
            _pgOptionsBuilder = new();
            _pgOptionsBuilder.DatabaseName = "ReservationSeasonPricesTest";

            using (PgDbContext dbContext = new PgDbContext(_pgOptionsBuilder))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            try
            {
                _seasonPriceRepository = new SeasonPriceSqlRepository(_pgOptionsBuilder);
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
        public async Task TestNotExist()
        {
            SeasonPrice? seasonPrice = await _seasonPriceRepository.Get(Guid.Empty);
            Assert.True(seasonPrice == null);
        }

        [Fact]
        public async Task TestAdd()
        {
            SeasonPrice newSeasonPrice = new();

            newSeasonPrice.Id = Guid.NewGuid();
            newSeasonPrice.RoomTypeId = Guid.NewGuid();
            newSeasonPrice.Multiplier = 0.1f;
            newSeasonPrice.DateFrom = new DateOnly(2026, 4, 30);
            newSeasonPrice.DateTo = new DateOnly(2026, 5, 10);

            Guid id = Guid.Empty;
            // добавление записи отеля
            try
            {
                id = await _seasonPriceRepository.Add(newSeasonPrice);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.Equal(id, newSeasonPrice.Id);

            SeasonPrice? findedSeasonPrice = await _seasonPriceRepository.Get(newSeasonPrice.Id);
            Assert.NotNull(findedSeasonPrice);

            await _seasonPriceRepository.Remove(id);

            findedSeasonPrice = await _seasonPriceRepository.Get(newSeasonPrice.Id);
            Assert.True(findedSeasonPrice == null);
        }

        [Fact]
        public async Task TestUpdate()
        {
            SeasonPrice newSeasonPrice = new();

            newSeasonPrice.Id = Guid.NewGuid();
            newSeasonPrice.RoomTypeId = Guid.NewGuid();
            newSeasonPrice.Multiplier = 0.1f;
            newSeasonPrice.DateFrom = new DateOnly(2026, 4, 30);
            newSeasonPrice.DateTo = new DateOnly(2026, 5, 10);

            try
            {
                await _seasonPriceRepository.Add(newSeasonPrice);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            SeasonPrice? findedSeasonPrice = await _seasonPriceRepository.Get(newSeasonPrice.Id);
            Assert.NotNull(findedSeasonPrice);

            // проверка обновления
            newSeasonPrice.Multiplier = 0.5f;

            try
            {
                await _seasonPriceRepository.Update(newSeasonPrice);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            findedSeasonPrice = await _seasonPriceRepository.Get(newSeasonPrice.Id);
            Assert.NotNull(findedSeasonPrice);

            Assert.Equal(0.5f, findedSeasonPrice.Multiplier);

            // проверка даты от позже даты от
            newSeasonPrice.DateFrom = new DateOnly(2026, 5, 30);
            try
            {
                await _seasonPriceRepository.Update(newSeasonPrice);
                Assert.Fail("Дата от не должна быть позже даты до.");
            }
            catch (Exception)
            {
                
            }

            newSeasonPrice.DateTo = new DateOnly(2026, 8, 31);
            try
            {
                await _seasonPriceRepository.Update(newSeasonPrice);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            await _seasonPriceRepository.Remove(findedSeasonPrice.Id);
        }

        [Fact]
        public async Task TestRemove()
        {
            SeasonPrice newSeasonPrice = new();

            newSeasonPrice.Id = Guid.NewGuid();
            newSeasonPrice.RoomTypeId = Guid.NewGuid();
            newSeasonPrice.Multiplier = 0.1f;
            newSeasonPrice.DateFrom = new DateOnly(2026, 4, 30);
            newSeasonPrice.DateTo = new DateOnly(2026, 5, 10);

            try
            {
                await _seasonPriceRepository.Add(newSeasonPrice);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            SeasonPrice? findedSeasonPrice = await _seasonPriceRepository.Get(newSeasonPrice.Id);
            Assert.NotNull(findedSeasonPrice);

            //---------------------------------------------------------------------

            SeasonPrice newSeasonPrice2 = new();

            newSeasonPrice2.Id = Guid.NewGuid();
            newSeasonPrice2.RoomTypeId = newSeasonPrice.RoomTypeId;
            newSeasonPrice2.Multiplier = 0.1f;
            newSeasonPrice2.DateFrom = new DateOnly(2026, 4, 30);
            newSeasonPrice2.DateTo = new DateOnly(2026, 5, 10);

            try
            {
                await _seasonPriceRepository.Add(newSeasonPrice2);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            SeasonPrice? findedSeasonPrice2 = await _seasonPriceRepository.Get(newSeasonPrice2.Id);
            Assert.NotNull(findedSeasonPrice2);

            try
            {
                await _seasonPriceRepository.Remove(newSeasonPrice.Id);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            SeasonPrice? removedSeasonPrice = await _seasonPriceRepository.Get(newSeasonPrice.Id);
            Assert.True(removedSeasonPrice == null);

            // проверить то, что осталась вторая запись, то есть что удалена только одна запись
            findedSeasonPrice2 = await _seasonPriceRepository.Get(newSeasonPrice2.Id);
            Assert.NotNull(findedSeasonPrice2);
        }

        [Fact]
        public async Task TestGetByParams()
        {
            SeasonPrice newSeasonPrice = new();

            newSeasonPrice.Id = Guid.NewGuid();
            newSeasonPrice.RoomTypeId = Guid.NewGuid();
            newSeasonPrice.Multiplier = 0.1f;
            newSeasonPrice.DateFrom = new DateOnly(2026, 4, 30);
            newSeasonPrice.DateTo = new DateOnly(2026, 5, 10);

            // добавление записи отеля
            try
            {
                await _seasonPriceRepository.Add(newSeasonPrice);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            IReadOnlyList<SeasonPrice> seasonPrices = new List<SeasonPrice>();

            try
            {
                seasonPrices = await _seasonPriceRepository.Get(sp => sp.Multiplier <= 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.True(seasonPrices.Count == 0);

            // несовпадение по году, совпадение по месяцу и дню
            try
            {
                var testDate = new DateOnly(2999, 5, 1);
                ISeasonPriceService seasonPriceService = new SeasonPriceService(_seasonPriceRepository);
                seasonPrices = await seasonPriceService.GetByDate(newSeasonPrice.RoomTypeId, testDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.True(seasonPrices.Count == 0);

            // несовпадение по году, несовпадение по месяцу, совпадение по дню
            try
            {
                var testDate = new DateOnly(2999, 12, 1);
                ISeasonPriceService seasonPriceService = new SeasonPriceService(_seasonPriceRepository);
                seasonPrices = await seasonPriceService.GetByDate(newSeasonPrice.RoomTypeId, testDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.True(seasonPrices.Count == 0);

            // несовпадение по году, несовпадение по месяцу, несовпадение по дню
            try
            {
                var testDate = new DateOnly(2999, 12, 15);
                ISeasonPriceService seasonPriceService = new SeasonPriceService(_seasonPriceRepository);
                seasonPrices = await seasonPriceService.GetByDate(newSeasonPrice.RoomTypeId, testDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.True(seasonPrices.Count == 0);

            // совпадение по году, несовпадение по месяцу, несовпадение по дню
            try
            {
                var testDate = new DateOnly(2026, 12, 15);
                ISeasonPriceService seasonPriceService = new SeasonPriceService(_seasonPriceRepository);
                seasonPrices = await seasonPriceService.GetByDate(newSeasonPrice.RoomTypeId, testDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.True(seasonPrices.Count == 0);

            // несовпадение по году, совпадение по месяцу, несовпадение по дню
            try
            {
                var testDate = new DateOnly(2026, 5, 15);
                ISeasonPriceService seasonPriceService = new SeasonPriceService(_seasonPriceRepository);
                seasonPrices = await seasonPriceService.GetByDate(newSeasonPrice.RoomTypeId, testDate);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.True(seasonPrices.Count == 0);

            // совпадение по году, совпадение по месяцу, совпадение по дню
            for (DateOnly date = newSeasonPrice.DateFrom; date <= newSeasonPrice.DateTo; date = date.AddDays(1))
            {
                try
                {
                    ISeasonPriceService seasonPriceService = new SeasonPriceService(_seasonPriceRepository);
                    seasonPrices = await seasonPriceService.GetByDate(newSeasonPrice.RoomTypeId, date);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }

                Assert.True(seasonPrices.Count == 1);
            }

            var addedSeasonPrice = seasonPrices.First();
            Assert.Equal(newSeasonPrice.Multiplier, addedSeasonPrice.Multiplier);

            await _seasonPriceRepository.Remove(newSeasonPrice.Id);
        }
    }
}
