using Hotels.Domain.Entities;

namespace Hotels.Domain.Repositories
{
    public interface IRoomTypesRepository
    {
        public Task<IReadOnlyList<RoomType>> GetAllByHotel(Guid hotelId);
        public Task<IReadOnlyList<RoomType>> GetAllByName(Guid hotelId, string name);
        public Task Remove(Guid roomTypeId);
        public Task Update(RoomType roomType);
        public Task Add(RoomType roomType);
        public Task<RoomType?> Get(Guid roomTypeId);

    }
}
