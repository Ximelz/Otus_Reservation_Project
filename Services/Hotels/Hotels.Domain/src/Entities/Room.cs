
namespace Hotels.Domain.Entities
{
	/// <summary>
	/// Сущность: Номер в отеле
	/// </summary>
	public class Room
	{
		public long Id { get; set; } = 0;
		public long HotelId { get; set; } = 0;
		public Hotel? Hotel { get; set; } = null;
		/// <summary>
		/// Порядковый номер
		/// </summary>
		public string Number { get; set; } = string.Empty;
		/// <summary>
		/// Вместимость - количество людей, которые могут свободно проживать в номере
		/// </summary>
		public int Capacity { get; set; } = 1;
		/// <summary>
		/// Уровень комфорта
		/// </summary>
		public int ComfortLevel { get; set; } = 1;
		public decimal Price { get; set; } = decimal.Zero;
	}
}
