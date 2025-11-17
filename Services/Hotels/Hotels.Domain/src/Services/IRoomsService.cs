using System.Collections.Generic;
using Hotels.Domain.src.Entities;

namespace Hotels.Domain.src.Services
{
    /// <summary>
    /// Менеджер номеров
    /// </summary>
    internal interface IRoomsService
    {
        void Add(Room room);
        void Remove(long roomId);
        void RemoveAll(long hotelId);
        void Update(Room room);
        Room Get(long roomId);

        IReadOnlyList<Room> GetAllByHotel(long hotelId);
        IReadOnlyList<Room> GetAllByComfortLevel(long hotelId, int minComfortLevel, int maxComfortLevel);
        IReadOnlyList<Room> GetAllByCapacity(long hotelId, int minCapacity, int maxCapacity);
        IReadOnlyList<Room> GetAllByPrice(long hotelId, decimal minPrice, decimal maxPrice);
    }
}
