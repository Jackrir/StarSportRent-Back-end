using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers.db
{
    [Route("api/db/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository repository;

        public UserController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if(role == "admin" || role == "worker")
                {
                    IEnumerable<User> user = await this.repository.GetRangeAsync<User>(true, x => true);
                    foreach(User item in user)
                    {
                        item.Bookings = null;
                        item.Rents = null;
                        item.Token = null;
                    }
                    return this.Ok(user.ToArray());
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    User user = await this.repository.GetAsync<User>(true, x => x.UserId == id);
                    if (user == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "User not found." });
                    }
                    user.Bookings = null;
                    user.Rents = null;
                    user.Token = null;
                    return this.Ok(user);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    User chakingByEmailUser = await this.repository.GetAsync<User>(true, x => x.Email == user.Email);
                    if (chakingByEmailUser != null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Mail is used." });
                    }
                    User newUser = new User
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Role = user.Role,
                        Password = service.GetHashString(user.Password)
                    };

                    User usertoken = await this.repository.AddAsync<User>(newUser);
                    await this.repository.AddAsync<Token>(new Token { UserId = usertoken.UserId });

                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    User oldUser = await this.repository.GetAsync<User>(true, x => x.UserId == user.UserId);
                    if (oldUser == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "User not found." });
                    }

                    oldUser.Name = user.Name;
                    oldUser.Role = user.Role;
                    oldUser.Email = user.Email;

                    await this.repository.UpdateAsync<User>(oldUser);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    User user = await this.repository.GetAsync<User>(true, x => x.UserId == id);
                    await this.repository.DeleteAsync<User>(user);
                    Token newtoken = await this.repository.GetAsync<Token>(true, x => x.UserId == id);
                    await this.repository.DeleteAsync<Token>(newtoken);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }
    }
}
