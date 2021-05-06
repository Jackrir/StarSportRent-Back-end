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
    public class Auth : IAuth
    {
        private readonly IRepository repository;

        public Auth(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<(AuthWeb, string)> LoginAsyncWeb(string email, string password)
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

            if (user.Role != "worker" && user.Role != "admin")
            {
                return (null, "No worker");
            }

            int role = 1;
            if(user.Role == "admin")
            {
                role = 0;
            } 
            else if(user.Role == "worker")
            {
                role = 1;
            }
            AuthentificationResult result = await service.GenerateAuthenticationResult(user);

            return (new AuthWeb() {Token = result.Token, RefreshToken = result.RefreshToken, Role = role }, "ok");
        }

        public async Task<(AuthResult, string)> LoginAsync(string email, string password)
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

            AuthentificationResult result = await service.GenerateAuthenticationResult(user);
            return (new AuthResult(){Name = user.Name, Token = result.Token, RefreshToken = result.RefreshToken }, "ok");
        }

        public async Task<(AuthResult, string)> RegistrationAsync(string email, string password, string name)
        {
            var user = await this.repository.GetAsync<User>(true, x => x.Email == email);
            if (user != null)
            {
                return (null, "Email is used");
            }
            AuthService service = new AuthService(repository);
            User newUser = new User()
            {
                Email = email,
                Password = service.GetHashString(password),
                Name = name,
                Role = "user"
            };
            await this.repository.AddAsync<User>(newUser);
            AuthentificationResult result = await service.GenerateAuthenticationResult(newUser);
            return (new AuthResult() { Name = newUser.Name, Token = result.Token, RefreshToken = result.RefreshToken }, "ok");
        }
    }
}
