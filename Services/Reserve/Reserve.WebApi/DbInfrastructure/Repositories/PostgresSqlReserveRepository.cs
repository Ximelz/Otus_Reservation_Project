using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var user = db.Set<PersonReserveModel>().Find(reserve.UserReserve.Id);
                var room = db.Set<RoomReserveModel>().Find(reserve.RoomReserve.Id);
                var hotel = db.Set<HotelReserveModel>().Find(reserve.RoomReserve.Hotel.Id);

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

        public Task<List<Reserve>> GetListAsync(Func<Reserve, bool> predicate, CancellationToken ct)
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
                                         .ToList());
            }
        }

        public Task<List<Reserve>> GetAllAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                return Task.FromResult(db.Reserves
                                         .Include(u => u.UserReserve)
                                         .Include(u => u.RoomReserve)
                                         .ThenInclude(u => u.Hotel)
                                         .ToList()
                                         .MapListFromModel());
            }
        }
    }
}
