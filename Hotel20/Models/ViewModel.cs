using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel20.Models
{
    public class RoomServiceViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<SerVice> Services { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }
}