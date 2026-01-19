using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    public class RoomsSqlRepository : IRoomsRepository
    {
        private readonly DbContextOptions<SqlDatabaseContext> _dbContextOptions;

        public RoomsSqlRepository(DbContextOptions<SqlDatabaseContext> dbContextOptions) 
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task Add(Room room)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                await dbContext.AddAsync(room);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(long roomId)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Room>().AsQueryable().Where(r => r.Id == roomId);
                await query.ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(Room room)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                var query = dbContext.Set<Room>().Remove(room);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Room room)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                Room? existedRoom = await Get(room.Id);
                if (existedRoom == null)
                {
                    return;
                }

                dbContext.Update(room);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Room?> Get(long roomId)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                return await dbContext.Set<Room>().SingleOrDefaultAsync(r => r.Id == roomId);
            }
        }

        public async Task<IReadOnlyList<Room>> GetAllByHotel(long hotelId)
        {
            using (SqlDatabaseContext dbContext = new(_dbContextOptions))
            {
                return await dbContext.Set<Room>().Where(r => r.HotelId == hotelId).ToListAsync();
            }
        }
    }
}
