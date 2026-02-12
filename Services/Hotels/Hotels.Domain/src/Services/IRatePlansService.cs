
using Hotels.Domain.Entities;

namespace Hotels.Domain.Services
{
    public interface IRatePlansService
    {
        Task<RatePlan?> Get(Guid id);
        Task<IReadOnlyList<RatePlan>> Get(Func<RatePlan, bool> predicate);
        Task Add(RatePlan ratePlan);
        Task Update(RatePlan ratePlan);
        Task Remove(Guid id);
    }
}
