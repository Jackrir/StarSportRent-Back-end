using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.API.Requests
{
    public class LoginRequest
    {
        public string login { get; set; }
        public string password { get; set; }
    }
}
