
using Core.Entities;

namespace Domain.src.Core.Repositories
{
    /// <summary>
    /// Хранилище номеров гостиницы
    /// </summary>
    internal interface IRoomsRepository
    {
        public void Add(Room room);
        public void RemoveRoom(long roomId);
        public void UpdateRoom(Room room);
        public Room GetRoom(long roomId);
        public IReadOnlyList<Room> GetHotelRooms(long hotelId);
    }
}
