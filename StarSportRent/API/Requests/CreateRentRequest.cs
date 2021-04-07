using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.API.Requests
{
    public class CreateRentRequest
    {
        public string data { get; set; }
        public DateTime time { get; set; }
        public int id { get; set; }
    }
}
