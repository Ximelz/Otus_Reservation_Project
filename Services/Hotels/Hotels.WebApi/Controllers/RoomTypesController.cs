using Hotels.Domain.Entities;
using Hotels.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.WebApi.Controllers
{
    [Route("api/roomType")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private IRoomTypesService _roomTypesService;

        public RoomTypesController(IRoomTypesService roomTypesService)
        {
            _roomTypesService = roomTypesService;
        }

        // POST api/roomType
        [HttpPost]
        public async Task Post(RoomType roomType)
        {
            if (await _roomTypesService.Get(roomType.Id) == null)
            {
                await _roomTypesService.Add(roomType);
            }
            else
            {
                await _roomTypesService.Update(roomType);
            }
        }

        // DELETE api/roomType/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(id, out guid))
            {
                return;
            }

            await _roomTypesService.Remove(guid);
        }

        // GET api/roomType/5
        [HttpGet("{id}")]
        public async Task<RoomType?> Get(string id)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(id, out guid))
            {
                return null;
            }

            return await _roomTypesService.Get(guid);
        }

        // GET api/roomType/hotel/5
        [HttpGet("roomType/hotel/{hotelId}")]
        public async Task<IReadOnlyList<RoomType>> GetByCountry(string hotelId)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(hotelId, out guid))
            {
                return [];
            }

            return await _roomTypesService.GetAllByHotel(guid);
        }

        // GET api/roomType/hotel/5/typeName/name
        [HttpGet("roomType/hotel/{hotelId}/typeName/{name}")]
        public async Task<IReadOnlyList<RoomType>> GetByName(string hotelId, string name)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(hotelId, out guid))
            {
                return [];
            }

            return await _roomTypesService.GetAllByName(guid, name);
        }
    }
}
