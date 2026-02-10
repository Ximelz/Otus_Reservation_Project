using Hotels.Domain.Entities;

namespace Hotels.Domain.Services
{
    /// <summary>
    /// Менеджер номеров
    /// </summary>
    public interface IRoomsService
    {
        Task Add(Room room);
        Task Remove(Guid roomId);
        Task Update(Room room);
        Task<Room?> Get(Guid roomId);

        Task<IReadOnlyList<Room>> GetAllByHotel(Guid hotelId);
        Task<IReadOnlyList<Room>> GetAllByComfortLevel(Guid hotelId, int minComfortLevel, int maxComfortLevel);
        Task<IReadOnlyList<Room>> GetAllByCapacity(Guid hotelId, int minCapacity, int maxCapacity);
        Task<IReadOnlyList<Room>> GetAllByPrice(Guid hotelId, decimal minPrice, decimal maxPrice);
    }
}
