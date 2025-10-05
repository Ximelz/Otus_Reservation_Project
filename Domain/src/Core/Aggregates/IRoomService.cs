
using Core.Entities;

namespace Domain.src.Core.Aggregates
{
    /// <summary>
    /// Менеджер номеров
    /// </summary>
    internal interface IRoomService
    {
        public void Add(Room room);
        public void RemoveRoom(long roomId);
        public void RemoveRooms(long hotelId);
        public void UpdateRoom(Room room);
        public Room getRoom(long roomId);
        public IReadOnlyList<Room> getHotelRooms(long hotelId);
    }
}
