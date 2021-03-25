using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.API.Responses
{
    public class AuthentificationResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
