
using Core.Entities;

namespace Domain.src.Core.Repositories
{
    /// <summary>
    /// Хранилище номеров гостиницы
    /// </summary>
    internal interface IRoomsRepository
    {
        public void Add(Room room);
        public void Remove(long roomId);
        public void Update(Room room);
        public Room Get(long roomId);
        public IReadOnlyList<Room> GetAllByHotel(long hotelId);
    }
}
