
using Hotels.Domain.Entities;

namespace Hotels.Domain.Repositories
{
    public interface ISeasonPriceRepository
    {
        Task<Guid> Add(SeasonPrice seasonPrice);
        Task Remove(Guid priceId);
        Task Update(SeasonPrice seasonPrice);
        Task<SeasonPrice?> Get(Guid seasonPriceId);

        Task<IReadOnlyList<SeasonPrice>> Get(Func<SeasonPrice, bool> predicate);
    }
}
