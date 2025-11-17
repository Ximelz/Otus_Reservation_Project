using Hotels.Domain.src.Entities;

namespace Hotels.Domain.src.Repositories
{
    public interface IHotelsRepository
    {
        Task Add(Hotel hotel);
        Task Remove(long hotelId);
        Task Update(Hotel hotel);
        Task<Hotel?> Get(long hotelId);

        Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId);
        Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars);
    }
}
