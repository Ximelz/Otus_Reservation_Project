using Hotels.Domain.src.Entities;
using Hotels.Domain.src.Repositories;
using Hotels.Domain.src.Services;

namespace Hotels.Infrastructure.Pg.src.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly IHotelsRepository _hotelsRepository;

        public HotelsService(IHotelsRepository hotelsRepository)
        {
            _hotelsRepository = hotelsRepository;
        }

        public async Task Add(Hotel hotel)
        {
            if (await IsExist(hotel.Id))
            {
                // todo log "Hotel {hotel.Name} is existed"
                return;
            }

            await _hotelsRepository.Add(hotel);
        }

        public async Task Remove(long hotelId)
        {
            if (!await IsExist(hotelId))
            {
                // todo log "Hotel is not removed. It is absent."
                return;
            }

            await _hotelsRepository.Remove(hotelId);
        }

        public async Task Update(Hotel hotel)
        {
            if (!await IsExist(hotel.Id))
            {
                // todo log "Hotel {hotel.Name} is not updated. It is absent."
                return;
            }

            await _hotelsRepository.Update(hotel);
        }

        public async Task<Hotel?> Get(long hotelId)
        {
            return await _hotelsRepository.Get(hotelId);
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId)
        {
            return await _hotelsRepository.GetAllByCountry(countryId);
        }

        public async Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars)
        {
            return await _hotelsRepository.GetAllByStars(stars);
        }

        private async Task<bool> IsExist(long id)
        {
            Hotel? existedHotel = await _hotelsRepository.Get(id);
            return (existedHotel != null);
        }
    }
}
