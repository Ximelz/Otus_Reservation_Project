using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class PostgresSqlReserveRepository : IReserveRepository
    {
        public PostgresSqlReserveRepository(IDbContextFactory<ReservDbContext> dbContext) => this.dbContext = dbContext;
        private readonly IDbContextFactory<ReservDbContext> dbContext;

        public Task AddAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            using (var db = dbContext.CreateDbContext())
            {
                db.Reserves.Add(reserve.MapToModel());
                db.SaveChanges();
                return Task.CompletedTask;
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
    }
}
