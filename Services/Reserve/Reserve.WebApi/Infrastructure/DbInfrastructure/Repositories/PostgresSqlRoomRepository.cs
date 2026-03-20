using Microsoft.EntityFrameworkCore;

namespace ReservService
{
    public class PostgresSqlRoomRepository : IRoomRepository
    {
        public PostgresSqlRoomRepository(IDbContextFactory<ReservDbContext> dbContext) => this.dbContext = dbContext;
        private readonly IDbContextFactory<ReservDbContext> dbContext;
        public async Task AddRoom(RoomReserve room, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                var hotel = db.Hotels.Find(room.Hotel.Id);

                var newRoom = room.MapToModel();

                if (hotel != null)
                    newRoom.Hotel = hotel;

                var t = db.Rooms.Add(newRoom);
                await db.SaveChangesAsync();
            }
        }

        public Task<RoomReserve?> GetRoom(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                var findRoom = db.Rooms.Find(id);
                RoomReserve? room = null;
                if (findRoom != null)
                    room = findRoom.MapFromModel();

                return Task.FromResult(room);
            }
        }

        public Task<IReadOnlyList<RoomReserve>> GetRooms(Func<RoomReserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                IReadOnlyList<RoomReserve> rooms = db.Rooms
                                                     .Include(u => u.Hotel)
                                                     .ToList()
                                                     .MapListFromModel()
                                                     .Where(predicate)
                                                     .ToList();
                return Task.FromResult(rooms);
            }
        }

        public Task RemoveRoom(RoomReserve room, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                db.Rooms.Remove(room.MapToModel());
                db.SaveChanges();
                return Task.CompletedTask;
            }
        }
    }
}
