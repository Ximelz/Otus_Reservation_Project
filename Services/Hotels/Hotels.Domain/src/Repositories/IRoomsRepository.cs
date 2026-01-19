
using Hotels.Domain.Entities;

namespace Hotels.Domain.Repositories
{
    /// <summary>
    /// Хранилище номеров гостиницы
    /// </summary>
    public interface IRoomsRepository
    {
        Task Add(Room room);
        Task Remove(long roomId);
        Task Remove(Room room);
        Task Update(Room room);
        Task<Room?> Get(long roomId);

        Task<IReadOnlyList<Room>> GetAllByHotel(long hotelId);
    }
}
