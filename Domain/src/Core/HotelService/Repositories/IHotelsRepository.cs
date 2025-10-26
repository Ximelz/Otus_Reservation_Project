
using Domain.src.Core.HotelService.Entities;

namespace Domain.src.Core.HotelService.Repositories
{
    internal interface IHotelsRepository
    {
        public void Add(Hotel hotel);
        public void Remove(long hotelId);
        public void Update(Hotel hotel);
        public Hotel Get(long hotelId);

        public IReadOnlyList<Hotel> GetAllByCountry(int countryId);
        public IReadOnlyList<Hotel> GetAllByStars(int stars);
    }
}
