
namespace Hotels.Domain.Entities
{
    public class RoomType
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid HotelId { get; set; } = Guid.Empty;
        public string Name { get; set; } = "";
        public string Description { get; set; } = string.Empty;
        /// <summary>
		/// Вместимость - количество людей, которые могут свободно проживать в номере
		/// </summary>
		public int Capacity { get; set; } = 1;
    }
}
