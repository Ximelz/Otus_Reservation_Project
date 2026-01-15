using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
                var query = dbContext.Set<Room>()
                            .Where(r => r.Id == roomId);
                await query.ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(Room room)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = dbContext.Set<Room>().Remove(room);
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
                return await dbContext.Set<Room>().SingleOrDefaultAsync(r => r.Id == roomId);
            }
        }

        public async Task<IReadOnlyList<Room>> GetAllByParameters(Guid hotelId,
                                                                  string comfortName = "",
                                                                  int capacity = 1,
                                                                  double minPrice = 0,
                                                                  double maxPrice = double.MaxValue)
        {
            using (PgDbContext dbContext = new(_pgContextOptions))
            {
                var query = from room in dbContext.Set<Room>()
                            join roomType in dbContext.Set<RoomType>()
                            on room.TypeId equals roomType.Id
                            where (room.HotelId == hotelId)
                                    && (string.IsNullOrEmpty(comfortName) || roomType.Name.Contains(comfortName))
                                    && (capacity == 0 || roomType.Capacity == capacity)
                            select room;

                return await query.ToListAsync();
            }
        }
    }
}
