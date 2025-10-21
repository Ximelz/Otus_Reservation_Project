using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    [Table("RoomReserve")]
    public class RoomReserveModel
    {
        public Guid Id { get; set; }
        public string RoomNumb { get; set; }
        public long Price { get; set; }
        public HotelReserveModel Hotel { get; set; }
        public int adultCount { get; set; }
        public int childCount { get; set; }
        public RoomReserveStatus Status { get; set; }
    }
}
