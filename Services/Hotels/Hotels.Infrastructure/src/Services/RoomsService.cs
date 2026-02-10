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

        public async Task<IReadOnlyList<Room>> GetAllByParameters(Guid hotelId,
                                                                  string typeName = "",
                                                                  int capacity = 1,
                                                                  double minPrice = 0,
                                                                  double maxPrice = double.MaxValue)
        {
            return await _roomsRepository.GetAllByParameters(hotelId, typeName, capacity, minPrice, maxPrice);
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
