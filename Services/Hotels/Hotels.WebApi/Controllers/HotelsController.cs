using Hotels.Domain.Entities;
using Hotels.Domain.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotels.WebApi.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IHotelsService _hotelsService;

        public HotelsController(IHotelsService hotelsService)
        {
            _hotelsService = hotelsService;
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public async Task<Hotel?> Get(long id)
        {
            return await _hotelsService.Get(id);
        }

        // POST api/<HotelsController>
        [HttpPost]
        public async Task Post(Hotel value)
        {
            await _hotelsService.Add(value);
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _hotelsService.Remove(id);
        }

        // GET hotels/byCountryId/5
        [HttpGet("byCountry/{countryId}")]
        public async Task<IReadOnlyList<Hotel>> GetByCountry(int countryId)
        {
            return await _hotelsService.GetAllByCountry(countryId);
        }

        // GET hotels/byStars/5
        [HttpGet("byStars/{stars}")]
        public async Task<IReadOnlyList<Hotel>> GetByStars(int stars)
        {
            return await _hotelsService.GetAllByStars([stars]);
        }
    }
}
