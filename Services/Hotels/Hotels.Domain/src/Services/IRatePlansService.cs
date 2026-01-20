
using Hotels.Domain.Entities;

namespace Hotels.Domain.Services
{
    public interface IRatePlansService
    {
        Task<IReadOnlyList<RatePlan>> Get(Guid id = default, Guid hotelId = default, Guid roomTypeId = default, 
                                          string name = "", double minPrice = 0.0, double maxPrice = 0.0);
        Task Add(RatePlan ratePlan);
        Task Update(RatePlan ratePlan);
        Task Remove(Guid id);
    }
}
