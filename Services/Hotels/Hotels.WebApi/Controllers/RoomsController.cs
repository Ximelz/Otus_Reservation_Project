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

        public RoomsController(IRoomsService roomsService)
        {
            _roomsService = roomsService;
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
            return await _roomsService.GetAllByHotel(id);
        }
    }
}
