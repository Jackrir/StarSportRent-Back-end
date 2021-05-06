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
    public class ItemsInRentController : ControllerBase
    {
        private readonly IRepository repository;

        public ItemsInRentController(IRepository repository)
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
                    IEnumerable<ItemsInRent> itemsInRent = await this.repository.GetRangeAsync<ItemsInRent>(true, x => true);
                    foreach (ItemsInRent el in itemsInRent)
                    {
                        el.Item = null;
                        el.Rent = null;
                    }
                    return this.Ok(itemsInRent.ToArray());
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpGet("{idRent}/{idItem}")]
        public async Task<IActionResult> GetId(int idRent, int idItem)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    ItemsInRent itemsInRent = await this.repository.GetAsync<ItemsInRent>(true, x => x.RentId == idRent && x.ItemId == idItem);
                    if (itemsInRent == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "ItemsInRent not found." });
                    }
                    itemsInRent.Rent = null;
                    itemsInRent.Item = null;
                    return this.Ok(itemsInRent);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemsInRent itemsInRent)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    ItemsInRent newItemsInRent = new ItemsInRent
                    {
                        RentId = itemsInRent.RentId,
                        ItemId = itemsInRent.ItemId
                    };

                    await this.repository.AddAsync<ItemsInRent>(newItemsInRent);

                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }

            
        }

        [HttpDelete("{idRent}/{idItem}")]
        public async Task<IActionResult> Delete(int idRent, int idItem)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    ItemsInRent itemsInRent = await this.repository.GetAsync<ItemsInRent>(true, x => x.RentId == idRent && x.ItemId == idItem);
                    await this.repository.DeleteAsync<ItemsInRent>(itemsInRent);
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
