
namespace Hotels.Domain.Entities
{
	/// <summary>
	/// Сущность: Номер в отеле
	/// </summary>

	public class Room
	{
		public Guid Id { get; set; } = Guid.Empty;
		public Guid HotelId { get; set; } = Guid.Empty;
		/// <summary>
		/// Порядковый номер
		/// </summary>
		public string Number { get; set; } = string.Empty;
		/// <summary>
		/// Доступен ли номер для бронирования на стороне отеля.
		/// Например, если в номере планируется сделать ремонт, то заблокировать его для дальнейших броней.
		/// </summary>
		public bool IsEnabled { get; set; } = true;
		/// <summary>
		/// Уровень комфорта
		/// </summary>
		public Guid TypeId { get; set; } = Guid.Empty;
	}
}
