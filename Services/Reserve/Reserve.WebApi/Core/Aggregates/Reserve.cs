
using System.ComponentModel.DataAnnotations;

namespace ReservService
{
	public class Reserve
    {
		public Reserve(Guid Id, PersonReserve UserReserve, RoomReserve RoomReserve, DateTime CheckIn, DateTime CheckOut, StatusReserve Status, PersonsCount Persons, double Cost)
		{
			this.Id = Id;
			this.UserReserve = UserReserve;
			this.RoomReserve = RoomReserve;
            this.CheckIn = CheckIn;
            this.CheckOut = CheckOut;
            this.Status = Status;
			this.Persons = Persons;
			this.Cost = Cost;
		}
        public Guid Id { get; private set; }
        public PersonReserve UserReserve { get; private set; }
        public RoomReserve RoomReserve { get; private set; }
        public double Cost { get; private set; }
		public DateTime CheckIn { get; private set; }
		public DateTime CheckOut { get; private set; }
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
			Cost = currentCost;

			if (Cost >= RoomReserve.Price)
				Status = StatusReserve.Active;
		}
		public void ChangeDates(DateTime CheckIn, DateTime CheckOut)
		{
			this.CheckIn = CheckIn;
			this.CheckOut = CheckOut;
		}

        public override bool Equals(object obj)
        {
            if (obj is Reserve other)
            {
                return this.UserReserve.Equals(other.UserReserve) &&
                       this.RoomReserve.Equals(other.RoomReserve) &&
                       this.Persons.Equals(other.Persons) &&
                       this.CheckIn == other.CheckIn &&
                       this.CheckOut == other.CheckOut &&
                       this.Cost == other.Cost &&
                       this.Status == other.Status &&
                       this.Id == other.Id;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(UserReserve.GetHashCode(),
															  RoomReserve.GetHashCode(),
															  Persons.GetHashCode(),
															  Id,
															  Status,
															  CheckIn,
															  CheckOut,
															  Cost);
    }
}
