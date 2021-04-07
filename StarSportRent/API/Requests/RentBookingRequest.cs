using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.API.Requests
{
    public class RentBookingRequest
    {
        public int id { get; set; }
        public DateTime time { get; set; }
    }
}
