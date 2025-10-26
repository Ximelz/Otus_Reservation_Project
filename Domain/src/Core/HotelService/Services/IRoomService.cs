
using Core.Entities;

namespace Domain.src.Core.Services
{
    /// <summary>
    /// Менеджер номеров
    /// </summary>
    internal interface IRoomService
    {
        public void Add(Room room);
        public void Remove(long roomId);
        public void RemoveAll(long hotelId);
        public void Update(Room room);
        public Room Get(long roomId);
        public IReadOnlyList<Room> GetAllByHotel(long hotelId);
    }
}
