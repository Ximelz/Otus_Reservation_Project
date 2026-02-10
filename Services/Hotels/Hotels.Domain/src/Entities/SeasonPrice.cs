
namespace Hotels.Domain.Entities
{
    public class SeasonPrice
    {
        public Guid Id {  get; set; }
        public Guid RoomTypeId { get; set; }
        public DateOnly DateFrom { get; set; }
        public DateOnly DateTo { get; set; }
        public float Multiplier { get; set; }
    }
}
