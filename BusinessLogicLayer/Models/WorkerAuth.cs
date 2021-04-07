using BusinessLogicLayer.API.Responses;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class WorkerAuth : IWorkerAuth
    {
        private readonly IRepository repository;

        public WorkerAuth(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<(AuthentificationResult, string)> LoginAsync(string email, string password)
        {
            var user = await this.repository.GetAsync<User>(true, x => x.Email == email);
            if (user == null)
            {
                return (null, "User unknown");
            }
            AuthService service = new AuthService(repository);
            string hashPasswod = service.GetHashString(password);
            if (user.Password != hashPasswod)
            {
                return (null, "Invalid Password");
            }

            if (user.Role != "worker")
            {
                return (null, "No worker");
            }

            return (await service.GenerateAuthenticationResult(user), "ok");
        }
    }
}
