using Hotels.Domain.Entities;

namespace Hotels.Domain.Repositories
{
    public interface IHotelsRepository
    {
        Task<long> Add(Hotel hotel);
        Task Remove(long hotelId);
        Task Update(Hotel hotel);
        Task<Hotel?> Get(long hotelId);

        Task<IReadOnlyList<Hotel>> GetAllByCountry(int countryId);
        Task<IReadOnlyList<Hotel>> GetAllByStars(HashSet<int> stars);
    }
}
