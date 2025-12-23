using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Domain.Services;

namespace Hotels.Infrastructure.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsService(IRoomsRepository roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }

        public async Task Add(Room room)
        {
            await _roomsRepository.Add(room);
        }

        public async Task<Room?> Get(long roomId)
        {
            return await _roomsRepository.Get(roomId);
        }

        public async Task<IReadOnlyList<Room>> GetAllByHotel(long hotelId)
        {
            return await _roomsRepository.GetAllByHotel(hotelId);
        }

        public Task<IReadOnlyList<Room>> GetAllByCapacity(long hotelId, int minCapacity, int maxCapacity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Room>> GetAllByComfortLevel(long hotelId, int minComfortLevel, int maxComfortLevel)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Room>> GetAllByPrice(long hotelId, decimal minPrice, decimal maxPrice)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(long roomId)
        {
            await _roomsRepository.Remove(roomId);
        }

        public async Task RemoveAll(long hotelId)
        {
            await _roomsRepository.Remove(-1);
        }

        public async Task Update(Room room)
        {
            await _roomsRepository.Update(room);
        }
    }
}
