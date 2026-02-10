using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Infrastructure.Repositories
{
    public class RoomsSqlRepository : IRoomsRepository
    {
        private readonly PgDbContextOptions _pgContextOptions;

        public RoomsSqlRepository(PgDbContextOptions dbContextOptions) 
        {
            _pgContextOptions = dbContextOptions;
        }

        public async Task Add(Room room)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                room.Id = Guid.NewGuid();
                await dbContext.AddAsync(room);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(Guid roomId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.Rooms
                            .Where(r => r.Id == roomId);
                await query.ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(Room room)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.Rooms.Remove(room);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(Room room)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
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

        public async Task<Room?> Get(Guid roomId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                return await dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId);
            }
        }

        public async Task<IReadOnlyList<Room>> GetAllByHotel(Guid hotelId)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                return await dbContext.Rooms
                             .AsQueryable()
                             .Where(r => r.HotelId == hotelId)
                             .ToListAsync();
            }
        }
    }
}
