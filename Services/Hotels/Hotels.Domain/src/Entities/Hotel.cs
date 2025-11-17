
namespace Hotels.Domain.src.Entities
{
    /// <summary>
	/// Сущность: Отель
	/// </summary>
    public class Hotel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
