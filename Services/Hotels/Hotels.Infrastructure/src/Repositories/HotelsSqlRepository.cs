
using Hotels.Domain.src.Entities;
using Hotels.Domain.src.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    internal class HotelsSqlRepository : IHotelsRepository
    {
        private readonly DbContext _dbContext;

        public HotelsSqlRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Hotel hotel)
        {
            await _dbContext.AddAsync(hotel);
        }

        public async Task<Hotel?> Get(long hotelId)
        {
            var query = _dbContext.Set<Hotel>().AsQueryable();
            return await query.SingleOrDefaultAsync(h => h.Id == hotelId);
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId)
        {
            var query = _dbContext.Set<Hotel>().AsQueryable();
            return await query.Where(h => h.CountryId == countryId).ToListAsync();
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars)
        {
            var query = _dbContext.Set<Hotel>().AsQueryable();
            return await query.Where(h => stars.Contains(h.Stars)).ToListAsync();
        }

        public async Task Remove(long hotelId)
        {
            var query = _dbContext.Set<Hotel>().AsQueryable();
            query.Where(h => h.Id == hotelId);
            await query.ExecuteDeleteAsync();
        }

        public async Task Update(Hotel hotel)
        {
            Hotel? repHotel = await Get(hotel.Id);
            if (repHotel == null)
            {
                // todo log hotel not found
                return;
            }

            repHotel.Name = hotel.Name;
            repHotel.Phone = hotel.Phone;
            repHotel.Email = hotel.Email;
            repHotel.Address = hotel.Address;
            repHotel.Stars = hotel.Stars;
            repHotel.Description = hotel.Description;
            repHotel.CountryId = hotel.CountryId;

            await _dbContext.SaveChangesAsync();
        }
    }
}
