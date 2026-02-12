
using Hotels.Domain.Entities;
using Hotels.Domain.Repositories;
using Hotels.Domain.Services;

namespace Hotels.Infrastructure.Services
{
    public class RoomTypesService : IRoomTypesService
    {
        private readonly IRoomTypesRepository _roomTypesRepository;

        public RoomTypesService(IRoomTypesRepository roomTypesRepository)
        {
            _roomTypesRepository = roomTypesRepository;
        }

        public async Task<IReadOnlyList<RoomType>> GetAllByHotel(Guid hotelId)
        {
            return await _roomTypesRepository.GetAllByHotel(hotelId);
        }

        public async Task<IReadOnlyList<RoomType>> GetAllByName(Guid hotelId, string name)
        {
            return await _roomTypesRepository.GetAllByName(hotelId, name);
        }

        public async Task Remove(Guid roomTypeId)
        {
            await _roomTypesRepository.Remove(roomTypeId);
        }

        public async Task Update(RoomType roomType)
        {
            await _roomTypesRepository.Update(roomType);
        }

        public async Task Add(RoomType roomType)
        {
            await _roomTypesRepository.Add(roomType);
        }

        public async Task<RoomType?> Get(Guid roomTypeId)
        {
            return await _roomTypesRepository.Get(roomTypeId);
        }
    }
}
