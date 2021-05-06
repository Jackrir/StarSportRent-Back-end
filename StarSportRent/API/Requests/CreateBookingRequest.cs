using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.API.Requests
{
    public class CreateBookingRequest
    {
        public int id { get; set; }
        public int count { get; set; }
    }
}
