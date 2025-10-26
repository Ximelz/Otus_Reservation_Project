
using Core.HotelService.Entities;

namespace Domain.src.Core.HotelService.Services
{
    /// <summary>
    /// Менеджер номеров
    /// </summary>
    internal interface IRoomsService
    {
        public void Add(Room room);
        public void Remove(long roomId);
        public void RemoveAll(long hotelId);
        public void Update(Room room);
        public Room Get(long roomId);

        public IReadOnlyList<Room> GetAllByHotel(long hotelId);
        public IReadOnlyList<Room> GetAllByComfortLevel(long hotelId, int minComfortLevel, int maxComfortLevel);
        public IReadOnlyList<Room> GetAllByCapacity(long hotelId, int minCapacity, int maxCapacity);
        public IReadOnlyList<Room> GetAllByPrice(long hotelId, decimal minPrice, decimal maxPrice);
    }
}
