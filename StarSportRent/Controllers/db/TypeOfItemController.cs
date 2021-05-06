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
    public class TypeOfItemController : ControllerBase
    {
        private readonly IRepository repository;

        public TypeOfItemController(IRepository repository)
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
                if (role == "admin")
                {
                    IEnumerable<TypeOfItem> type = await this.repository.GetRangeAsync<TypeOfItem>(true, x => true);
                    foreach (TypeOfItem item in type)
                    {
                        item.Category = null;
                        item.TypeItems = null;
                    }
                    return this.Ok(type.ToArray());
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
                    TypeOfItem type = await this.repository.GetAsync<TypeOfItem>(true, x => x.TypeId == id);
                    if (type == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Type not found." });
                    }
                    type.Category = null;
                    type.TypeItems = null;
                    return this.Ok(type);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TypeOfItem type)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    TypeOfItem newType = new TypeOfItem
                    {
                        CategoryId = type.CategoryId,
                        Name = type.Name,
                        Info = type.Info
                    };

                    await this.repository.AddAsync<TypeOfItem>(newType);

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
        public async Task<IActionResult> Update([FromBody] TypeOfItem type)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    TypeOfItem oldType = await this.repository.GetAsync<TypeOfItem>(true, x => x.TypeId == type.TypeId);
                    if (oldType == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Type not found." });
                    }

                    oldType.CategoryId = type.CategoryId;
                    oldType.Name = type.Name;
                    oldType.Info = type.Info;

                    await this.repository.UpdateAsync<TypeOfItem>(oldType);
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
                    TypeOfItem type = await this.repository.GetAsync<TypeOfItem>(true, x => x.TypeId == id);
                    await this.repository.DeleteAsync<TypeOfItem>(type);
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
