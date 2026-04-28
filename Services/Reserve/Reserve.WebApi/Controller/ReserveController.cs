using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.IntegrationEvents.Reservations;

namespace ReservService
{
    [Route("api/reservations")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _service;
        private readonly IPublishEndpoint _publishEndpoint;
        public ReserveController(IReserveService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUser(Guid id, CancellationToken ct)
        {
            try
            {
                IReadOnlyList<Reserve> reserves = await _service.GetReservesByUserId(id, ct);
                return Ok(reserves);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ReserveDto reserveDto, CancellationToken ct)
        {
            try
            {
                var freeRoomFlag = (await _service.GetReservesByRoomId(reserveDto.roomId, ct));
                if (freeRoomFlag.Any(x => x.CheckIn <= reserveDto.checkOut && x.CheckOut >= reserveDto.checkIn))
                    return BadRequest("Данные даты заняты!");

                IReserveFactory factory = new ReserveFactory();

                var reserve = factory.CreateReserve(reserveDto);

                await _service.RegisterReserve(reserve, ct);
                await _publishEndpoint.Publish(reserve.CreateEvent(), ct);
                return Ok(reserve);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/checkIn")]
        public async Task<IActionResult> PutUpdateCheckIn(Guid id, [FromBody] DateTime CheckIn, CancellationToken ct)
        {
            try
            {
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.ChangeDates(CheckIn, reserve.CheckOut);
                await _service.UpdateReserve(reserve, ct);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/checkOut")]
        public async Task<IActionResult> PutUpdateCheckOut(Guid id, [FromBody] DateTime CheckOut, CancellationToken ct)
        {
            try
            {
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.ChangeDates(reserve.CheckIn, CheckOut);
                await _service.UpdateReserve(reserve, ct);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/cost")]
        public async Task<IActionResult> PutUpdateCost(Guid id, [FromBody] long cost, CancellationToken ct)
        {
            try
            {
                var reserve = await _service.GetReserveById(id, ct);

                if (reserve == null)
                    return NotFound();

                reserve.MakeAPay(cost);
                await _service.UpdateReserve(reserve, ct);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> PutCancel([FromBody] string Reason, Guid id, CancellationToken ct)
        {
            try
            {
                var reserve = await _service.GetReserveById(id, ct);

                await _service.CancelReserve(id, ct);
                await _publishEndpoint.Publish(reserve.CancellEvent(Reason), ct);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}