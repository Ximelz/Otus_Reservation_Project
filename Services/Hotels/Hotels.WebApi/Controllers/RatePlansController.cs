using Hotels.Domain.Entities;
using Hotels.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.WebApi.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class RatePlansController : ControllerBase
    {
        private IRatePlansService _ratesService;

        public RatePlansController(IRatePlansService ratesService)
        {
            _ratesService = ratesService;
        }

        // GET api/rates/5
        [HttpGet("{id}")]
        public async Task<IReadOnlyList<RatePlan>> Get(Guid id)
        {
            return await _ratesService.Get(rp => rp.Id == id);
        }

        // POST api/rates
        [HttpPost]
        public async Task Post(RatePlan rate)
        {
            if (await _ratesService.Get(rp => rp.Id == rate.Id) == null)
            {
                await _ratesService.Add(rate);
            }
            else
            {
                await _ratesService.Update(rate);
            }
        }

        // DELETE api/rates/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _ratesService.Remove(id);
        }

        // GET api/rates/hotel/5
        [HttpGet("hotel/{id}")]
        public async Task<IReadOnlyList<RatePlan>> GetAllByHotel(Guid id)
        {
            return await _ratesService.Get(rp => rp.HotelId == id);
        }

        // GET api/rates/hotel/5/roomType/1
        [HttpGet("hotel/{hotelId}/roomType/{roomTypeId}")]
        public async Task<IReadOnlyList<RatePlan>> GetAllByHotel(Guid hotelId, Guid roomTypeId)
        {
            return await _ratesService.Get(rp => rp.HotelId == hotelId && rp.RoomTypeId == roomTypeId);
        }
    }
}
