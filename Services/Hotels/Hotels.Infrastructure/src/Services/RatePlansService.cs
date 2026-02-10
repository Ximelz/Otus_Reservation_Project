using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Domain.Services;

namespace Hotels.Infrastructure.Services
{
    public class RatePlansService : IRatePlansService
    {
        private readonly IRatePlansRepository _ratesRepository;

        public RatePlansService(IRatePlansRepository ratesRepository)
        {
            _ratesRepository = ratesRepository;
        }

        public async Task Add(RatePlan ratePlan)
        {
            await _ratesRepository.Add(ratePlan);
        }

        public async Task<RatePlan?> Get(Guid id)
        {
            return await _ratesRepository.Get(id);
        }

        public async Task<IReadOnlyList<RatePlan>> Get(Func<RatePlan, bool> predicate)
        {
            return await _ratesRepository.Get(predicate);
        }

        public async Task Remove(Guid id)
        {
            await _ratesRepository.Remove(id);
        }

        public async Task Update(RatePlan ratePlan)
        {
            await _ratesRepository.Update(ratePlan);
        }
    }
}
