
using Domain.src.Core.HotelService.Entities;

namespace Domain.src.Core.HotelService.Repositories
{
    internal interface IPhotosRepository
    {
        public void Add(Photo photo);
        public void Remove(long photoId);
        public void Update(Photo photo);
        public Photo Get(long photoId);
        public Photo Get(long hotelId, long? roomId);

        public IReadOnlyList<Photo> GetAllByHotel(long hotelId);
        public IReadOnlyList<Photo> GetAllByRoom(long roomId);
    }
}
