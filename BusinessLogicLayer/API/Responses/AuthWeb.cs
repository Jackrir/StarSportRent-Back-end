using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.API.Responses
{
    public class AuthWeb
    {
        public int Role { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
