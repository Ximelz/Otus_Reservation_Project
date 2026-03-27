using System;
using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
                //ratePlan.Id = Guid.NewGuid();
                await dbContext.AddAsync(ratePlan);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<RatePlan?> Get(Guid id)
        {
            IReadOnlyList<RatePlan> rates = await Get(rp => rp.Id == id);

            if (rates.Count != 1)
            {
                // error
                return null;
            }

            return rates.First();
        }

        public Task<IReadOnlyList<RatePlan>> Get(Func<RatePlan, bool> predicate)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.RatePlans.Where(predicate)
                                .OrderBy(rp => rp.HotelId)
                                .OrderBy(rp => rp.BasePrice);

                return Task.FromResult<IReadOnlyList<RatePlan>>(query.ToList());
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
                var existedRate = await Get(rp => rp.Id == ratePlan.Id);
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
