using Hotels.Domain.Entities;

namespace Hotels.Domain.Services
{
    public interface IHotelsService
    {
        Task Add(Hotel hotel);
        Task Remove(Guid hotelId);
        Task Update(Hotel hotel);
        Task<Hotel?> Get(Guid hotelId);

        Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId);
        Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars);
    }
}
