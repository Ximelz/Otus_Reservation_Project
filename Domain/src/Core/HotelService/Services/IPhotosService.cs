using Domain.src.Core.HotelService.Entities;

namespace Domain.src.Core.HotelService.Services
{
    internal interface IPhotosService
    {
        public void Add(Photo room);
        public void Remove(long photoId);
        public void Update(Photo photo);
        public Photo Get(long photoId);
        public Photo Get(long hotelId, long? roomId);

        public IReadOnlyList<Photo> GetAllByHotel(long hotelId);
        public IReadOnlyList<Photo> GetAllByRoom(long roomId);
    }
}
