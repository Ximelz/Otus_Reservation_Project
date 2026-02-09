using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class PersonReserve
    {
        public PersonReserve(Guid Id, string Email, string Phone, string Name, PersonReserveContactType ContactType)
        {
            this.Id = Id;
            this.Email = Email;
            this.Phone = Phone;
            this.Name = Name;
            this.ContactType = ContactType;
        }
        public readonly Guid Id;
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Name { get; private set; }
        public PersonReserveContactType ContactType { get; private set; }
        public void RenamePerson(string newName) => Name = newName;
        public void ChangeContactType(PersonReserveContactType newType) => ContactType = newType;
        public void ChangePhone(string newPhone) => Phone = newPhone;
        public void CHangeEmail(string newEmail) => Email = newEmail;


        public override bool Equals(object obj)
        {
            if (obj is PersonReserve other)
            {
                return this.ContactType == other.ContactType &&
                       this.Name == other.Name &&
                       this.Email == other.Email &&
                       this.Phone == other.Phone &&
                       this.Id == other.Id;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(ContactType,
                                                              Id,
                                                              Name,
                                                              Email,
                                                              Phone);
    }
}
