
using Hotels.Domain.Entities;

namespace Hotels.Domain.Services
{
    public interface ISeasonPriceService
    {
        Task<Guid> Add(SeasonPrice seasonPrice);
        Task Remove(Guid priceId);
        Task Update(SeasonPrice seasonPrice);
        Task<SeasonPrice?> Get(Guid seasonPriceId);

        Task<IReadOnlyList<SeasonPrice>> Get(Func<SeasonPrice, bool> predicate);
        Task<IReadOnlyList<SeasonPrice>> GetByDate(Guid roomTypeId, DateOnly date);
    }
}
