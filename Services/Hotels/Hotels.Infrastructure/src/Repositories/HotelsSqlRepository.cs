
using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    public class HotelsSqlRepository : IHotelsRepository
    {
        private readonly DbContextOptions<SqlDatabaseContext> _dbContextOptions;

        public HotelsSqlRepository(DbContextOptions<SqlDatabaseContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<long> Add(Hotel hotel)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                await dbContext.AddAsync(hotel);
                await dbContext.SaveChangesAsync();

                return hotel.Id;
            }
        }

        public async Task<Hotel?> Get(long hotelId)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Hotel>().AsQueryable();
                return await query.SingleOrDefaultAsync(h => h.Id == hotelId);
            }
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Hotel>().AsQueryable();
                return await query.Where(h => h.CountryId == countryId).ToListAsync();
            }
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Hotel>().AsQueryable();
                return await query.Where(h => stars.Contains(h.Stars)).ToListAsync();
            }
        }

        public async Task Remove(long hotelId)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Hotel>().AsQueryable();
                query.Where(h => h.Id == hotelId);
                await query.ExecuteDeleteAsync();
            }
        }

        public async Task Update(Hotel hotel)
        {
            Hotel? repHotel = await Get(hotel.Id);
            if (repHotel == null)
            {
                // todo log hotel not found
                return;
            }

            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                repHotel.Name = hotel.Name;
                repHotel.Phone = hotel.Phone;
                repHotel.Email = hotel.Email;
                repHotel.Address = hotel.Address;
                repHotel.Stars = hotel.Stars;
                repHotel.Description = hotel.Description;
                repHotel.CountryId = hotel.CountryId;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
