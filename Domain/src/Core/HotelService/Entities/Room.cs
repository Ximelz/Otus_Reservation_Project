
namespace Core.Entities
{
	/// <summary>
	/// Уровень комфорта в номере
	/// </summary>
	public enum ComfortLevel
	{
		First,
		Second,
		Third
	}

	/// <summary>
	/// Номер в гостинице
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
		public ComfortLevel Level { get; set; }
		public decimal Price { get; set; }
	}
}
