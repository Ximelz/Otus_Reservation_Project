
using Hotels.Domain.Entities;

namespace Hotels.Domain.Repositories
{
    /// <summary>
    /// Хранилище номеров гостиницы
    /// </summary>
    public interface IRoomsRepository
    {
        Task Add(Room room);
        Task Remove(Guid roomId);
        Task Remove(Room room);
        Task Update(Room room);
        Task<Room?> Get(Guid roomId);

        Task<IReadOnlyList<Room>> GetAllByHotel(Guid hotelId);
        //Task<IReadOnlyList<Room>> GetAllBy(long hotelId, int capacitiy);
    }
}
