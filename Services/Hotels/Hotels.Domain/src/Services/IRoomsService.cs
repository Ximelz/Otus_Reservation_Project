using Hotels.Domain.Entities;

namespace Hotels.Domain.Services
{
    /// <summary>
    /// Менеджер номеров
    /// </summary>
    public interface IRoomsService
    {
        Task Add(Room room);
        Task Remove(long roomId);
        Task RemoveAll(long hotelId);
        Task Update(Room room);
        Task<Room?> Get(long roomId);

        Task<IReadOnlyList<Room>> GetAllByHotel(long hotelId);
        Task<IReadOnlyList<Room>> GetAllByComfortLevel(long hotelId, int minComfortLevel, int maxComfortLevel);
        Task<IReadOnlyList<Room>> GetAllByCapacity(long hotelId, int minCapacity, int maxCapacity);
        Task<IReadOnlyList<Room>> GetAllByPrice(long hotelId, decimal minPrice, decimal maxPrice);
    }
}
