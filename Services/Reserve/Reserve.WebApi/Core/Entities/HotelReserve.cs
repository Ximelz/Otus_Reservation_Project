namespace ReservService
{
    public class HotelReserve
    {
        public HotelReserve(Guid Id, string Name, string Email, string Phone, HotelReserveAddress Address)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
        }
        public Guid Id { get; private set; }
        public HotelReserveAddress Address { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public void RenameHotel(string newName) => Name = newName;
        public void ChangeEmail(string newEmail) => Email = newEmail;
        public void ChangePhone(string newPhone) => Phone = newPhone;

        public override bool Equals(object obj)
        {
            if (obj is HotelReserve other)
            {
                return this.Address.Equals(other.Address) &&
                       this.Name == other.Name &&
                       this.Email == other.Email &&
                       this.Phone == other.Phone &&
                       this.Id == other.Id;
            }
            
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Address.GetHashCode(),
                                                              Id,
                                                              Name,
                                                              Email,
                                                              Phone);
    }
}
