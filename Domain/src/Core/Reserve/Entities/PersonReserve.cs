using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
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
    }
}
