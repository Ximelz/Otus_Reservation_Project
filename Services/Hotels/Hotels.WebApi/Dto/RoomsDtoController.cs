using Hotels.Domain.Entities;
using Hotels.Domain.Services;
using Hotels.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.WebApi.Controllers
{
    [Route("gateway/room")]
    [ApiController]
    public class RoomsDtoController : Controller
    {
        private readonly IRoomsService _roomsService;
        private readonly IHotelsService _hotelsService;
        private readonly IRatePlansService _rateService;
        private readonly IRoomTypesService _roomTypesService;

        public RoomsDtoController(IRoomsService roomsService, IHotelsService hotelsService, IRatePlansService rateService, IRoomTypesService roomTypesService)
        {
            _roomsService = roomsService;
            _hotelsService = hotelsService;
            _rateService = rateService;
            _roomTypesService = roomTypesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> Get(Guid id)
        {
            try
            {
                var room = await _roomsService.Get(id);

                if (room == null)
                    return NotFound();
                
                var hotel = await _hotelsService.Get(room.HotelId);

                if (hotel == null)
                    return NotFound();

                var ratePlan = (await _rateService.Get(x => x.HotelId == hotel.Id)).FirstOrDefault();

                double price = 100.0;

                if (ratePlan != null)
                    price = (double)ratePlan.BasePrice;

                var roomType = await _roomTypesService.Get(room.TypeId);

                if (roomType == null)
                    return NotFound();

                var Dto = RoomDtoFactory.GetRoomDto(room, hotel, price, roomType.Capacity);

                return Ok(Dto);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
