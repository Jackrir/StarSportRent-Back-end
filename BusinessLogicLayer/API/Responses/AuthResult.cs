using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.API.Responses
{
    public class AuthResult
    {
        public string Name { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
