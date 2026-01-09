using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    public class HotelsSqlRepository : IHotelsRepository
    {
        private readonly DbContextOptions<PgDbContext> _dbContextOptions;

        public HotelsSqlRepository(DbContextOptions<PgDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<Guid> Add(Hotel hotel)
        {
            using (PgDbContext dbContext = new(_dbContextOptions))
            {
                hotel.Id = Guid.NewGuid();
                await dbContext.AddAsync(hotel);
                await dbContext.SaveChangesAsync();

                return hotel.Id;
            }
        }

        public async Task<Hotel?> Get(Guid hotelId)
        {
            using (PgDbContext dbContext = new(_dbContextOptions))
            {
                return await dbContext.Set<Hotel>().SingleOrDefaultAsync(h => h.Id == hotelId);
            }
        }
        
        public async Task Remove(Guid hotelId)
        {
            using (PgDbContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext
                                .Set<Hotel>().AsQueryable()
                                .Where(h => h.Id == hotelId);
                await query.ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Hotel hotel)
        {
            Hotel? repHotel = await Get(hotel.Id);
            if (repHotel == null)
            {
                return;
            }

            using (PgDbContext dbContext = new(_dbContextOptions))
            {
                repHotel.Name = hotel.Name;
                repHotel.Phone = hotel.Phone;
                repHotel.Email = hotel.Email;
                repHotel.Address = hotel.Address;
                repHotel.Stars = hotel.Stars;
                repHotel.Description = hotel.Description;
                repHotel.CountryId = hotel.CountryId;

                dbContext.Update(repHotel);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId)
        {
            using (PgDbContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Hotel>().AsQueryable();
                return await query
                            .Where(h => h.CountryId == countryId)
                            .ToListAsync();
            }
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars)
        {
            using (PgDbContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Hotel>().AsQueryable();
                return await query
                            .Where(h => stars.Contains(h.Stars))
                            .ToListAsync();
            }
        }

    }
}
