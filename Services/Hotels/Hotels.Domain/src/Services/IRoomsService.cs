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

        Task<IReadOnlyList<Room>> GetAllByParameters(Guid hotelId,
                                                     string comfortName = "",
                                                     int capacity = 1,
                                                     double minPrice = 0,
                                                     double maxPrice = double.MaxValue);
    }
}
