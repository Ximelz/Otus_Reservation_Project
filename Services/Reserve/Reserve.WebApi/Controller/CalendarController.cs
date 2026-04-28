using Microsoft.AspNetCore.Mvc;

namespace ReservService
{
    [Route("api/calendars")]
    public class CalendarController : ControllerBase
    {
        private readonly IReservesCalendarService _calendarService;
        public CalendarController(IReservesCalendarService calendarService) => _calendarService = calendarService;

        [HttpGet("{city}/byCity")]
        public async Task<IActionResult> GetByCity([FromBody] DatesDTO dates, string city)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var calendars = await _calendarService.GetReserveCalendarByCity(dates.date1, dates.date2, city, cts);
                return Ok(calendars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/byHotel")]
        public async Task<IActionResult> GetByHotel([FromBody] DatesDTO dates, Guid id)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var calendars = await _calendarService.GetReserveCalendarByHotel(dates.date1, dates.date2, id, cts);
                return Ok(calendars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{country}/byCountry")]
        public async Task<IActionResult> GetByCountry([FromBody] DatesDTO dates, string country)
        {
            try
            {
                CancellationToken cts = new CancellationToken();
                var calendars = await _calendarService.GetReserveCalendarByCountry(dates.date1, dates.date2, country, cts);
                return Ok(calendars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public class DatesDTO
        {
            public DateTime date1 { get; set; }
            public DateTime date2 { get; set; }
        }
    }
}
