using Microsoft.AspNetCore.Mvc;



namespace ReservService
{
    [Route("api/reservations")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _service;
        public ReserveController(IReserveService service) => _service = service;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUser(Guid id)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                IReadOnlyList<Reserve> reserves = await _service.GetReservesByUserId(id, ct);
                return Ok(reserves);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ReserveDto reserveDto)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                await _service.RegisterReserve(ReserveFactory.CreateReserve(reserveDto), ct);
                return Ok();
            }
            catch
            {
                return BadRequest();
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
                return Ok();
            }
            catch
            {
                return BadRequest();
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
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}/cost")]
        public async Task<IActionResult> PutUpdateCost(Guid id, [FromBody] long cost)
        {
            try
            {
                CancellationToken ct = new CancellationToken();
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.MakeAPay(cost);
                await _service.UpdateReserve(reserve, ct);
                return Ok();
            }
            catch
            {
                return BadRequest();
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
                return BadRequest();
            }
        }
    }
}