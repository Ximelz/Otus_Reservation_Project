using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class ReserveService : IReserveService
    {
        public ReserveService(IReserveRepository repository) => this.repository = repository;

        private readonly IReserveRepository repository;
        public async Task RegisterReserve(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await repository.AddAsync(reserve, ct);
        }

        public async Task CancelReserve(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var reserve = await repository.GetAsync(x => x.Id == id, ct);

            if (reserve == null)
                return;

            reserve.ChangeReserveStatus(StatusReserve.Cancel);
            await repository.UpdateAsync(reserve, ct);
        }

        public async Task UpdateReserve(Reserve reserve, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await repository.UpdateAsync(reserve, ct);
        }

        public async Task<IReadOnlyList<Reserve>> GetReservesByUserId(Guid userId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return await repository.GetListAsync(x => x.UserReserve.Id == userId, ct);
        }

        public async Task<IReadOnlyList<Reserve>> GetReservesByHotelId(Guid hotelId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return await repository.GetListAsync(x => x.RoomReserve.Hotel.Id == hotelId, ct);
        }

        public async Task<IReadOnlyList<Reserve>> GetReservesByRoomId(Guid roomId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();


            return await repository.GetListAsync(x => x.RoomReserve.Id == roomId, ct);
        }

        public async Task<Reserve?> GetReserveById(Guid reserveId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return await repository.GetAsync(x => x.Id == reserveId, ct);
        }
    }
}
