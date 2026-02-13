
using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    public class SeasonPriceSqlRepository : ISeasonPriceRepository
    {
        private readonly PgDbContextOptions _pgContextOptions;

        public SeasonPriceSqlRepository(PgDbContextOptions dbContextOptions)
        {
            _pgContextOptions = dbContextOptions;
        }

        public async Task<Guid> Add(SeasonPrice seasonPrice)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                seasonPrice.Id = Guid.NewGuid();
                await dbContext.AddAsync(seasonPrice);
                await dbContext.SaveChangesAsync();

                return seasonPrice.Id;
            }
        }

        public async Task<SeasonPrice?> Get(Guid seasonPriceId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                return await dbContext.SeasonPrices.SingleOrDefaultAsync(sp => sp.Id == seasonPriceId);
            }
        }

        public Task<IReadOnlyList<SeasonPrice>> Get(Func<SeasonPrice, bool> predicate)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.SeasonPrices.Where(predicate)
                                .OrderBy(rp => rp.RoomTypeId)
                                .OrderBy(rp => rp.DateFrom);

                return Task.FromResult<IReadOnlyList<SeasonPrice>>(query.ToList());
            }
        }

        public async Task Remove(Guid priceId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext
                                .SeasonPrices.AsQueryable()
                                .Where(sp => sp.Id == priceId);
                await query.ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(SeasonPrice seasonPrice)
        {
            SeasonPrice? xSeasonPrice = await Get(seasonPrice.Id);
            if (xSeasonPrice == null)
            {
                return;
            }

            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                xSeasonPrice.DateFrom = seasonPrice.DateFrom;
                xSeasonPrice.DateTo = seasonPrice.DateTo;
                xSeasonPrice.Multiplier = seasonPrice.Multiplier;

                dbContext.Update(xSeasonPrice);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
