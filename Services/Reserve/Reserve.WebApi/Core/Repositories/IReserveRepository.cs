
namespace ReservService
{
	public interface IReserveRepository
    {
		public Task AddAsync(Reserve reserve, CancellationToken ct);
		public Task UpdateAsync (Reserve reserve, CancellationToken ct);
		public Task DeleteAsync (Reserve reserve, CancellationToken ct);
		public Task<Reserve?> GetAsync(Func<Reserve, bool> predicate, CancellationToken ct);
		public Task<IReadOnlyList<Reserve>> GetListAsync(Func<Reserve, bool> predicate, CancellationToken ct);
        public Task<IReadOnlyList<Reserve>> GetAllAsync(CancellationToken ct);
    }
}
