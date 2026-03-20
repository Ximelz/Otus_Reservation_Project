using Microsoft.EntityFrameworkCore;

namespace ReservService
{
    public class PostgresSqlReserveRepository : IReserveRepository
    {
        public PostgresSqlReserveRepository(IDbContextFactory<ReservDbContext> dbContext) => this.dbContext = dbContext;
        private readonly IDbContextFactory<ReservDbContext> dbContext;

        public async Task AddAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                var user = db.Users.Find(reserve.UserReserve.Id);
                var room = db.Rooms.Find(reserve.RoomReserve.Id);
                var hotel = db.Hotels.Find(reserve.RoomReserve.Hotel.Id);

                var newReserve = reserve.MapToModel();

                if (user != null)
                    newReserve.UserReserve = user;

                if (room != null)
                    newReserve.RoomReserve = room;

                if (hotel != null)
                    newReserve.RoomReserve.Hotel = hotel;

                var t = db.Reserves.Add(newReserve);
                await db.SaveChangesAsync();
            }
        }

        public Task UpdateAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                db.Reserves.Update(reserve.MapToModel());
                db.SaveChanges();
                return Task.CompletedTask;
            }
        }

        public Task DeleteAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                db.Reserves.Remove(reserve.MapToModel());
                db.SaveChanges();
                return Task.CompletedTask;
            }
        }

        public Task<Reserve?> GetAsync(Func<Reserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                return Task.FromResult(db.Reserves
                                         .Include(u => u.UserReserve)
                                         .Include(u => u.RoomReserve)
                                         .ThenInclude(u => u.Hotel)
                                         .ToList()
                                         .MapListFromModel()
                                         .Where(predicate)
                                         .FirstOrDefault());
            }
        }

        public Task<IReadOnlyList<Reserve>> GetListAsync(Func<Reserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                IReadOnlyList<Reserve> reserves = db.Reserves.Include(u => u.UserReserve)
                                                             .Include(u => u.RoomReserve)
                                                             .ThenInclude(u => u.Hotel)
                                                             .ToList()
                                                             .MapListFromModel()
                                                             .Where(predicate)
                                                             .ToList();

                return Task.FromResult(reserves);
            }
        }

        public Task<IReadOnlyList<Reserve>> GetAllAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                IReadOnlyList<Reserve> reserves = db.Reserves.Include(u => u.UserReserve)
                                                             .Include(u => u.RoomReserve)
                                                             .ThenInclude(u => u.Hotel)
                                                             .ToList()
                                                             .MapListFromModel();
                return Task.FromResult(reserves);
            }
        }
    }
}
