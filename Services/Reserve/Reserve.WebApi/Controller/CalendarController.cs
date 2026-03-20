using Microsoft.AspNetCore.Mvc;

namespace ReservService
{
    [Route("api/calendars")]
    public class CalendarController : ControllerBase
    {
        private readonly IReservesCalendarService _calendarService;
        public CalendarController(IReservesCalendarService calendarService) => _calendarService = calendarService;

        [HttpGet("{city}/byCity")]
        public async Task<IActionResult> GetByCity([FromBody] DateTime date1, [FromBody] DateTime date2, string city)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var calendars = await _calendarService.GetReserveCalendarByCity(date1, date2, city, cts);
                return Ok(calendars);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}/byHotel")]
        public async Task<IActionResult> GetByHotel([FromBody] DateTime date1, [FromBody] DateTime date2, Guid id)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var calendars = await _calendarService.GetReserveCalendarByHotel(date1, date2, id, cts);
                return Ok(calendars);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{country}/byCountry")]
        public async Task<IActionResult> GetByCountry([FromBody] DateTime date1, [FromBody] DateTime date2, string country)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var calendars = await _calendarService.GetReserveCalendarByCountry(date1, date2, country, cts);
                return Ok(calendars);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
