using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    [Table("HotelReserve")]
    public class HotelReserveModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string postIndex { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string buildNumber { get; set; }
    }
}
