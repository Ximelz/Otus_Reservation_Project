using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;


namespace ReservService
{
    [Route("api/reservations")]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _service;
        public ReserveController(IReserveService service) => _service = service;

        [HttpGet("{id}/byRoom")]
        public async Task<List<Reserve>> GetByRoom(Guid id)
        {
            CancellationToken cts = new CancellationToken();
            return await _service.GetReservesByRoomId(id, cts);
        }

        [HttpGet("{id}/byHotel")]
        public async Task<List<Reserve>> GetByHotel(Guid id)
        {
            CancellationToken cts = new CancellationToken();
            return await _service.GetReservesByHotelId(id, cts);
        }

        [HttpGet("{id}")]
        public async Task<List<Reserve>> Get(Guid id)
        {
            CancellationToken cts = new CancellationToken();
            return await _service.GetReservesByUserId(id, cts);
        }

        [HttpPost]
        public async void Post(ReserveDto reserveDto)
        {
            CancellationToken ct = new CancellationToken();
            await _service.RegisterReserve(ReserveFactory.CreateReserve(reserveDto), ct);
        }

        [HttpPut("{id}/check-in/{CheckIn}")]
        public async void PutUpdateCheckIn(Guid id, DateTime CheckIn)
        {
            CancellationToken ct = new CancellationToken();
            var reserve = await _service.GetReserveById(id, ct);

            if (reserve == null)
                return;

            reserve.ChangeDates(CheckIn, reserve.CheckOut);
            await _service.UpdateReserve(reserve, ct);
        }

        [HttpPut("{id}/check-out/{CheckOut}")]
        public async void PutUpdateCheckOut(Guid id, DateTime CheckOut)
        {
            CancellationToken ct = new CancellationToken();
            var reserve = await _service.GetReserveById(id, ct);

            if (reserve == null)
                return;

            reserve.ChangeDates(reserve.CheckIn, CheckOut);
            await _service.UpdateReserve(reserve, ct);
        }

        [HttpPut("{id}/cost/{cost}")]
        public async void PutUpdateCost(Guid id, long cost)
        {
            CancellationToken ct = new CancellationToken();
            var reserve = await _service.GetReserveById(id, ct);

            if (reserve == null)
                return;

            reserve.MakeAPay(cost);
            await _service.UpdateReserve(reserve, ct);
        }

        [HttpPut("{id}/cancel")]
        public async void PutCancel(Guid id)
        {
            CancellationToken ct = new CancellationToken();
            await _service.CancelReserve(id, ct);
        }
    }
}
