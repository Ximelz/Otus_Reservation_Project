
namespace Domain.src.Core.HotelService.Entities
{
	/// <summary>
	/// Сущность: Номер в отеле
	/// </summary>
	public class Room
	{
		public long Id { get; set; }
		public long HotelId { get; set; }
		/// <summary>
		/// Порядковый номер
		/// </summary>
		public string Number { get; set; } = string.Empty;
		/// <summary>
		/// Вместимость - количество людей, которые могут свободно проживать в номере
		/// </summary>
		public int Capacity { get; set; }
		/// <summary>
		/// Уровень комфорта
		/// </summary>
		public int ComfortLevel { get; set; }
		public decimal Price { get; set; }
    }
}
