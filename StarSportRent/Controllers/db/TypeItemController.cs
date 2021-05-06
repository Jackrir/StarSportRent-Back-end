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
    public class TypeItemController : ControllerBase
    {
        private readonly IRepository repository;

        public TypeItemController(IRepository repository)
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
                    IEnumerable<TypeItem> typeItem = await this.repository.GetRangeAsync<TypeItem>(true, x => true);
                    foreach (TypeItem el in typeItem)
                    {
                        el.Item = null;
                        el.Type = null;
                    }
                    return this.Ok(typeItem.ToArray());
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
         
        }

        [HttpGet("{idType}/{idItem}")]
        public async Task<IActionResult> GetId(int idType, int idItem)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    TypeItem typeItem = await this.repository.GetAsync<TypeItem>(true, x => x.TypeId == idType && x.ItemId == idItem);
                    if (typeItem == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "TypeItem not found." });
                    }
                    typeItem.Item = null;
                    typeItem.Type = null;
                    return this.Ok(typeItem);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TypeItem typeItem)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    TypeItem newTypeItem = new TypeItem
                    {
                        TypeId = typeItem.TypeId,
                        ItemId = typeItem.ItemId
                    };

                    await this.repository.AddAsync<TypeItem>(newTypeItem);

                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpDelete("{idType}/{idItem}")]
        public async Task<IActionResult> Delete(int idType, int idItem)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    TypeItem typeItem = await this.repository.GetAsync<TypeItem>(true, x => x.TypeId == idType && x.ItemId == idItem);
                    await this.repository.DeleteAsync<TypeItem>(typeItem);
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
