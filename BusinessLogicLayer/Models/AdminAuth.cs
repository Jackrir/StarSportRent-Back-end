using BusinessLogicLayer.API.Responses;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class AdminAuth : IAdminAuth
    {
        private readonly IRepository repository;

        public AdminAuth(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<(AuthentificationResult,string)> LoginAsync(string email, string password)
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

            if (user.Role != "admin")
            {
                return (null, "No admin");
            }

            return (await service.GenerateAuthenticationResult(user), "ok");
        }
    }
}
