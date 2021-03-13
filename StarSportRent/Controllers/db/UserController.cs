using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<User>> Get()
        {
            IEnumerable<User> user = await this.repository.GetRangeAsync<User>(true, x => true);
            return user.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<User> GetId(int id)
        {
            User user = await this.repository.GetAsync<User>(true, x => x.UserId == id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            User chakingByEmailUser = await this.repository.GetAsync<User>(true, x => x.Email == user.Email);
            if (chakingByEmailUser != null)
            {
                throw new Exception("Mail is used.");
            }

            User newUser = new User
            {
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                Password = user.Password
            };

            User usertoken = await this.repository.AddAsync<User>(newUser);
            await this.repository.AddAsync<Token>(new Token { UserId = usertoken.UserId });

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            User oldUser = await this.repository.GetAsync<User>(true, x => x.UserId == user.UserId);
            if (oldUser == null)
            {
                throw new Exception("User not found.");
            }

            oldUser.Name = user.Name;
            oldUser.Password = user.Password;
            oldUser.Role = user.Role;

            await this.repository.UpdateAsync<User>(oldUser);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await this.repository.GetAsync<User>(true, x => x.UserId == id);
            await this.repository.DeleteAsync<User>(user);
            Token token = await this.repository.GetAsync<Token>(true, x => x.UserId == id);
            await this.repository.DeleteAsync<Token>(token);
            return this.Ok();
        }
    }
}
