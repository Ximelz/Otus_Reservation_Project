
using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    public class RoomTypesSqlRepository : IRoomTypesRepository
    {
        private readonly PgDbContextOptions _pgContextOptions;

        public RoomTypesSqlRepository(PgDbContextOptions pgContextOptions)
        {
            _pgContextOptions = pgContextOptions;
        }

        public async Task<IReadOnlyList<RoomType>> GetAllByHotel(Guid hotelId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                return await dbContext.RoomTypes
                             .AsQueryable()
                             .Where(r => r.HotelId == hotelId)
                             .ToListAsync();
            }
        }

        public async Task<IReadOnlyList<RoomType>> GetAllByName(Guid hotelId,string name)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                return await dbContext.RoomTypes
                             .AsQueryable()
                             .Where(r => 
                                r.HotelId == hotelId 
                                && r.Name.Contains(name))
                             .ToListAsync();
            }
        }

        public async Task Remove(Guid roomTypeId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.RoomTypes
                            .Where(r => r.Id == roomTypeId);
                await query.ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(RoomType roomType)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                dbContext.Update(roomType);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Add(RoomType roomType)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                //roomType.Id = Guid.NewGuid();
                await dbContext.AddAsync(roomType);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<RoomType?> Get(Guid roomTypeId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                return await dbContext.RoomTypes.SingleOrDefaultAsync(r => r.Id == roomTypeId);
            }
        }
    }
}
