
namespace Hotels.Domain.Entities
{
    /// <summary>
	/// Сущность: Отель
	/// </summary>
    public class Hotel
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public int Stars { get; set; } = 0;
        public string Description { get; set; } = "";
        public int CountryId { get; set; } = 0;
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";

        public List<Room> Rooms { get; set; } = new();
    }
}
