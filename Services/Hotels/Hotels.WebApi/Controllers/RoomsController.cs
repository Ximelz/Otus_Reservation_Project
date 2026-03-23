using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Domain.Services;
using Hotels.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.WebApi.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private IRoomsService _roomsService;
        private IRatePlansService _ratePlansService;
        private ISeasonPriceService _seasonPriceService;

        public RoomsController(IRoomsService roomsService, IRatePlansService ratePlansService, ISeasonPriceService seasonPriceService)
        {
            _roomsService = roomsService;
            _ratePlansService = ratePlansService;
            _seasonPriceService = seasonPriceService;
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public async Task<Room?> Get(Guid id)
        {
            return await _roomsService.Get(id);
        }

        // POST api/<RoomsController>
        [HttpPost]
        public async Task Post(Room room)
        {
            if (await _roomsService.Get(room.Id) == null)
            {
                await _roomsService.Add(room);
            }
            else
            {
                await _roomsService.Update(room);
            }
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _roomsService.Remove(id);
        }

        // GET api/<RoomsController>/hotel/5
        [HttpGet("hotel/{id}")]
        public async Task<IReadOnlyList<Room>> GetAllByHotel(Guid id)
        {
            return await _roomsService.GetAllByParameters(hotelId: id);
        }

        // GET api/<RoomsController>/hotel/5
        [HttpGet("{id}/priceFor/year/{year}/month/{month}/day/{day}")]
        public async Task<double> GetPrice(Guid id, int year, int month, int day)
        {
            Room? room = await _roomsService.Get(id);
            if (room == null)
            {
                return 0;
            }

            float multiplier = 1.0f;
            var multipliers = await _seasonPriceService.GetByDate(room.TypeId, new DateOnly(year, month, day));
            if (multipliers.Count > 0)
            {
                multiplier = multipliers.First().Multiplier;
            }

            double price = 0;
            var rates = await _ratePlansService.Get(rp => rp.RoomTypeId == room.TypeId);
            if (rates.Count > 0)
            {
                price = (double)rates.First().BasePrice * multiplier;
            }

            return price;
        }
    }
}
