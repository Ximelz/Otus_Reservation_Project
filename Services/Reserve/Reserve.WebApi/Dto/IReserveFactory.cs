namespace ReservService
{
    public interface IReserveFactory
    {
        public Reserve CreateReserve(ReserveDto dto);
    }
}
