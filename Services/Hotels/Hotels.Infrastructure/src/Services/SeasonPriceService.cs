using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Domain.Services;

namespace Hotels.Infrastructure.Services
{
    public class SeasonPriceService : ISeasonPriceService
    {
        private readonly ISeasonPriceRepository _seasonPriceRepository;

        public SeasonPriceService(ISeasonPriceRepository seasonPriceRepository)
        {
            _seasonPriceRepository = seasonPriceRepository;
        }

        public async Task<Guid> Add(SeasonPrice seasonPrice)
        {
            if (await IsExist(seasonPrice.Id))
            {
                return Guid.Empty;
            }

            if (seasonPrice.RoomTypeId == Guid.Empty)
            {
                throw new Exception("Season price: room type is absent.");
            }

            await _seasonPriceRepository.Add(seasonPrice);
            return seasonPrice.Id;
        }

        public async Task<SeasonPrice?> Get(Guid seasonPriceId)
        {
            return await _seasonPriceRepository.Get(seasonPriceId);
        }

        public async Task<IReadOnlyList<SeasonPrice>> Get(Func<SeasonPrice, bool> predicate)
        {
            return await _seasonPriceRepository.Get(predicate);
        }

        public async Task Remove(Guid priceId)
        {
            if (!await IsExist(priceId))
            {
                return;
            }

            await _seasonPriceRepository.Remove(priceId);
        }

        public async Task Update(SeasonPrice seasonPrice)
        {
            if (!await IsExist(seasonPrice.Id))
            {
                return;
            }

            if (seasonPrice.DateTo < seasonPrice.DateFrom)
            {
                throw new Exception("Season price: dateTo earlier than dateFrom");
            }

            if (seasonPrice.Multiplier <= 0)
            {
                throw new Exception("Season price: multiplier is less than or equal to zero.");
            }

            await _seasonPriceRepository.Update(seasonPrice);
        }

        public async Task<IReadOnlyList<SeasonPrice>> GetByDate(Guid roomTypeId, DateOnly date)
        {
            return await _seasonPriceRepository.Get(sp => (sp.RoomTypeId == roomTypeId)
                                                          && (date.Year == sp.DateFrom.Year && date.Year == sp.DateTo.Year)
                                                          && ((date.Month > sp.DateFrom.Month
                                                               && date.Month < sp.DateTo.Month)
                                                              || (sp.DateFrom.Month == sp.DateTo.Month
                                                                  && date.Month == sp.DateTo.Month
                                                                  && date.Day >= sp.DateFrom.Day
                                                                  && date.Day <= sp.DateTo.Day)
                                                              || (date.Month == sp.DateFrom.Month
                                                                  && date.Month < sp.DateTo.Month
                                                                  && date.Day >= sp.DateFrom.Day)
                                                              || (date.Month == sp.DateTo.Month
                                                                  && date.Month > sp.DateFrom.Month
                                                                  && date.Day <= sp.DateTo.Day)));
        }

        private async Task<bool> IsExist(Guid id)
        {
            SeasonPrice? existedSeasonPrice = await _seasonPriceRepository.Get(id);
            return (existedSeasonPrice != null);
        }
    }
}
