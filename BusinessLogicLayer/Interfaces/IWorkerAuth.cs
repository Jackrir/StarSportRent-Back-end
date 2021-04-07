using BusinessLogicLayer.API.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IWorkerAuth
    {
        Task<(AuthentificationResult, string)> LoginAsync(string email, string password);
    }
}
