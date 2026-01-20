
namespace Hotels.Domain.Entities
{
    public class RatePlan
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid HotelId { get; set; } = Guid.Empty;
        public Guid RoomTypeId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0;
    }
}
