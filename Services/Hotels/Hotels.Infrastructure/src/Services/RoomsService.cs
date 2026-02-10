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

        public async Task<Room?> Get(Guid roomId)
        {
            return await _roomsRepository.Get(roomId);
        }

        public async Task<IReadOnlyList<Room>> GetAllByHotel(Guid hotelId)
        {
            return await _roomsRepository.GetAllByHotel(hotelId);
        }

        public Task<IReadOnlyList<Room>> GetAllByCapacity(Guid hotelId, int minCapacity, int maxCapacity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Room>> GetAllByComfortLevel(Guid hotelId, int minComfortLevel, int maxComfortLevel)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Room>> GetAllByPrice(Guid hotelId, decimal minPrice, decimal maxPrice)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(Guid roomId)
        {
            await _roomsRepository.Remove(roomId);
        }

        public async Task Update(Room room)
        {
            await _roomsRepository.Update(room);
        }
    }
}
