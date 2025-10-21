using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class InMemoryReserveRepository : IReserveRepository
    {
        private readonly List<Reserve> reserves;

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

        public Task<List<Reserve>> GetListAsync(Func<Reserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return Task.FromResult(reserves.Where(predicate).ToList());
        }

    }
}
