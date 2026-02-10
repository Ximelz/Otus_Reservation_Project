<<<<<<< HEAD
﻿
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
		/// Вместимость - количество людей, которые могут свободно проживать в номере
		/// </summary>
		public int Capacity { get; set; } = 1;
		/// <summary>
		/// Доступен ли номер для бронирования на стороне отеля.
		/// Например, если в номере планируется сделать ремонт, то заблокировать его для дальнейших броней.
		/// </summary>
		public bool IsEnabled { get; set; } = true;
		/// <summary>
		/// Уровень комфорта
		/// </summary>
		public int TypeId { get; set; } = 0;
	}
}
=======
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
>>>>>>> develop
