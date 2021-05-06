using BusinessLogicLayer.API.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuth
    {
        Task<(AuthWeb, string)> LoginAsyncWeb(string email, string password);
        Task<(AuthResult, string)> LoginAsync(string email, string password);
        Task<(AuthResult, string)> RegistrationAsync(string email, string password, string name);
    }
}
