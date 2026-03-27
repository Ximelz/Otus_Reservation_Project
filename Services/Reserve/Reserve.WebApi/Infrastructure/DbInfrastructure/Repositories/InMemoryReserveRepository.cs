using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class InMemoryReserveRepository : IReserveRepository
    {
        private readonly List<Reserve> reserves = new List<Reserve>();

        public Task AddAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (reserves.Where(r => r.Id == reserve.Id).Any())
                throw new ArgumentException("Бронь уже есть в репозитории!");

            reserves.Add(reserve);

            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            int index = reserves.FindIndex(r => r.Id == reserve.Id);
            reserves[index] = reserve;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (!reserves.Where(r => r.Id == reserve.Id).Any())
                throw new ArgumentException("Брони нет в репозитории!");
            
            reserves.Remove(reserve);

            return Task.CompletedTask;
        }

        public Task<Reserve?> GetAsync(Func<Reserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return Task.FromResult(reserves.Where(predicate).FirstOrDefault());
        }

        public Task<IReadOnlyList<Reserve>> GetListAsync(Func<Reserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            IReadOnlyList<Reserve> readonlyReserves= reserves.Where(predicate).ToList();
            return Task.FromResult(readonlyReserves);
        }

        public Task<IReadOnlyList<Reserve>> GetAllAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return Task.FromResult((IReadOnlyList<Reserve>)reserves);
        }
    }
}
