
namespace Domain.src.Core.HotelService.Entities
{
    internal class Photo
    {
        public long Id { get; set; }
        public long HotelId { get; set; }
        public long RoomId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
