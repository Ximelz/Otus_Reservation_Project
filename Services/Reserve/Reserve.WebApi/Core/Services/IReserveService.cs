
namespace ReservService
{
	public interface IReserveService
    {
		public Task RegisterReserve(Reserve reserve, CancellationToken ct);
		public Task CancelReserve(Guid id, CancellationToken ct);
		public Task UpdateReserve(Reserve reserve, CancellationToken ct);
		public Task<List<Reserve>> GetReservesByUserId(Guid userId, CancellationToken ct);
        public Task<List<Reserve>> GetReservesByHotelId(Guid hotelId, CancellationToken ct);
        public Task<List<Reserve>> GetReservesByRoomId(Guid roomId, CancellationToken ct);
        public Task<Reserve> GetReserveById(Guid reserveId, CancellationToken ct);
    }
}
