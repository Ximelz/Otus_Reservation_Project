using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hotels.Infrastructure.Repositories
{
    public class RatePlansSqlRepository : IRatePlansRepository
    {
        private readonly PgDbContextOptions _pgContextOptions;

        public RatePlansSqlRepository(PgDbContextOptions dbContextOptions)
        {
            _pgContextOptions = dbContextOptions;
        }

        public async Task Add(RatePlan ratePlan)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                ratePlan.Id = Guid.NewGuid();
                await dbContext.AddAsync(ratePlan);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<RatePlan>> Get(Guid id = default, Guid hotelId = default, 
                                                       Guid roomTypeId = default, string name = "", 
                                                       double minPrice = 0.0, double maxPrice = 0.0)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.RatePlans.Where(rp =>
                                 (id == default || rp.Id == id)
                              && (hotelId == default || rp.HotelId == hotelId)
                              && (roomTypeId == default || rp.RoomTypeId == roomTypeId)
                              && (name == "" || rp.Name.Contains(name))
                              && (minPrice == 0.0 || rp.Price >= minPrice)
                              && (maxPrice == 0.0 || rp.Price <= maxPrice)
                            )
                    .OrderBy(rp => rp.HotelId)
                    .OrderBy(rp => rp.Price);

                return await query.ToListAsync();
            }
        }

        public async Task Remove(Guid id)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = await dbContext.RatePlans
                                    .Where(rp => rp.Id == id)
                                    .ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(RatePlan ratePlan)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var existedRate = await Get(id: ratePlan.Id);
                if (existedRate.Count != 1)
                {
                    return;
                }

                dbContext.Update(ratePlan);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
