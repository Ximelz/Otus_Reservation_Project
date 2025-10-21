
using System.ComponentModel.DataAnnotations;

namespace Core.Reserve
{
	public class Reserve
    {
		public Reserve(Guid Id, PersonReserve UserReserve, RoomReserve RoomReserve, long Cost, TimeSpan ReserveTimeSpan, StatusReserve Status, PersonsCount Persons)
		{
			this.Id = Id;
			this.UserReserve = UserReserve;
			this.RoomReserve = RoomReserve;
			this.Cost = Cost;
			this.ReserveTimeSpan = ReserveTimeSpan;
			this.Status = Status;
			this.Persons = Persons;
			Cost = 0;
		}
        public readonly Guid Id;
        public readonly PersonReserve UserReserve;
        public readonly RoomReserve RoomReserve;
		public long Cost { get; private set; }
		public TimeSpan ReserveTimeSpan { get; private set; }
		public StatusReserve Status {  get; private set; }
		public PersonsCount Persons { get; private set; }
		
		public void ChangeReserveStatus(StatusReserve newStatus) => Status = newStatus;
		public void ChangePersonsCount(PersonsCount newPersonsCount)
		{
			Persons = newPersonsCount;
			Status = StatusReserve.AwaitPay;
		}
		public void MakeAPay(long currentCost)
		{
			Cost += currentCost;

			if (Cost >= RoomReserve.Price)
				Status = StatusReserve.Active;
		}
	}
}
