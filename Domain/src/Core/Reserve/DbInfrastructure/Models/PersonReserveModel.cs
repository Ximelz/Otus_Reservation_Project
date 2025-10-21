using Core.Reserve;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    [Table("PersonReserve")]
    public class PersonReserveModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public PersonReserveContactType ContactType { get; set; }
    }
}
