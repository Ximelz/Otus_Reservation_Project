
namespace Core.Reserve
{
	public interface IReserveRepository
    {
		public Task AddAsync(Reserve reserve, CancellationToken ct);
		public Task UpdateAsync (Reserve reserve, CancellationToken ct);
		public Task DeleteAsync (Reserve reserve, CancellationToken ct);
		public Task<Reserve?> GetAsync(Func<Reserve, bool> predicate, CancellationToken ct);
		public Task<List<Reserve>> GetListAsync(Func<Reserve, bool> predicate, CancellationToken ct);
	}
}
