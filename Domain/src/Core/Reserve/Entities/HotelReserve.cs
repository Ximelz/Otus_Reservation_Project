using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class HotelReserve
    {
        public HotelReserve(Guid Id, string Name, string Email, string Phone)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Phone = Phone;
        }
        public readonly Guid Id;
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public void RenameHotel(string newName) => Name = newName;
        public void ChangeEmail(string newEmail) => Email = newEmail;
        public void ChangePhone(string newPhone) => Phone = newPhone;
    }
}
