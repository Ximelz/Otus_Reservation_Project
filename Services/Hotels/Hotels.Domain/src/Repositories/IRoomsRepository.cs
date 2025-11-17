
using Hotels.Domain.src.Entities;

namespace Hotels.Domain.src.Repositories
{
    /// <summary>
    /// Хранилище номеров гостиницы
    /// </summary>
    internal interface IRoomsRepository
    {
        Task Add(Room room);
        Task Remove(long roomId);
        Task Update(Room room);
        Task<Room?> Get(long roomId);

        Task<IReadOnlyList<Room>> GetAllByHotel(long hotelId);
    }
}
