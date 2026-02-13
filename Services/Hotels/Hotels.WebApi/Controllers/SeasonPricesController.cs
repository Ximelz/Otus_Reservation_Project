using Hotels.Domain.Entities;
using Hotels.Domain.Services;
using Hotels.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.WebApi.Controllers
{
    [Route("api/seasonPrice")]
    [ApiController]
    public class SeasonPricesController : ControllerBase
    {
        private ISeasonPriceService _seasonPriceService;

        public SeasonPricesController(ISeasonPriceService seasonPriceService)
        {
            _seasonPriceService = seasonPriceService;
        }

        // POST api/seasonPrice
        [HttpPost]
        public async Task Post(SeasonPrice seasonPrice)
        {
            if (await _seasonPriceService.Get(seasonPrice.Id) == null)
            {
                await _seasonPriceService.Add(seasonPrice);
            }
            else
            {
                await _seasonPriceService.Update(seasonPrice);
            }
        }

        // DELETE api/seasonPrice/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(id, out guid))
            {
                return;
            }

            await _seasonPriceService.Remove(guid);
        }

        // GET api/seasonPrice/5
        [HttpGet("{id}")]
        public async Task<SeasonPrice?> Get(string id)
        {
            Guid guid = Guid.Empty;
            if (!Guid.TryParse(id, out guid))
            {
                return null;
            }

            return await _seasonPriceService.Get(guid);
        }

        // GET api/month/4/day/30/roomTypeId
        [HttpGet("month/{month}/day/{day}/roomType/{roomTypeId}")]
        public async Task<IReadOnlyList<SeasonPrice>> GetByDate(int month, int day, Guid roomTypeId)
        {
            return await _seasonPriceService.GetByDate(roomTypeId, new DateOnly(9999, month, day));
        }
    }
}
