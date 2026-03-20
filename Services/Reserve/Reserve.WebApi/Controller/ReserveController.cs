using Microsoft.AspNetCore.Mvc;


namespace ReservService
{
    [Route("api/reservations")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _service;
        public ReserveController(IReserveService service) => _service = service;

        [HttpGet("{id}/byRoom")]
        public async Task<ActionResult<List<Reserve>>> GetByRoom(Guid id)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var reserves = await _service.GetReservesByRoomId(id, cts);
                return Ok(reserves);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/byHotel")]
        public async Task<ActionResult<List<Reserve>>> GetByHotel(Guid id)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var reserves = await _service.GetReservesByHotelId(id, cts);
                return Ok(reserves);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/byUser")]
        public async Task<ActionResult<List<Reserve>>> GetByUser(Guid id)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var reserves = await _service.GetReservesByUserId(id, cts);
                return Ok(reserves);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserve>> Get(Guid id)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var reserves = await _service.GetReserveById(id, cts);
                return Ok(reserves);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(ReserveDto reserveDto)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                var reserve = ReserveFactory.CreateReserve(reserveDto);
                await _service.RegisterReserve(reserve, ct);
                return Ok(reserve);
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpPut("{id}/checkIn")]
        public async Task<IActionResult> PutUpdateCheckIn(Guid id, [FromBody] DateTime CheckIn)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.ChangeDates(CheckIn, reserve.CheckOut);
                await _service.UpdateReserve(reserve, ct);
                return Ok(reserve);
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpPut("{id}/checkOut")]
        public async Task<IActionResult> PutUpdateCheckOut(Guid id, [FromBody] DateTime CheckOut)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.ChangeDates(reserve.CheckIn, CheckOut);
                await _service.UpdateReserve(reserve, ct);
                return Ok(reserve);
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpPut("{id}/cost/{cost}")]
        public async Task<IActionResult> PutUpdateCost(Guid id, long cost)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.MakeAPay(cost);
                await _service.UpdateReserve(reserve, ct);
                return Ok(reserve);
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> PutCancel(Guid id)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                await _service.CancelReserve(id, ct);
                return Ok();
            }
            catch
            {
                return NoContent();
            }
        }
    }
}
