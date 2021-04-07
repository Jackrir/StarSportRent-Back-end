using BusinessLogicLayer.Models;
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
    public class ItemController : ControllerBase
    {
        private readonly IRepository repository;

        public ItemController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    IEnumerable<Item> item = await this.repository.GetRangeAsync<Item>(true, x => true);
                    foreach (Item el in item)
                    {
                        el.Bookings = null;
                        el.ItemsInRents = null;
                        el.Bookings = null;
                        el.Maintenances = null;
                    }
                    return this.Ok(item.ToArray());
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
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin" || role == "worker")
                {
                    Item item = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
                    if (item == null)
                    {
                        this.NotFound(new ErrorMessage { message = "Item not found." });
                    }
                    item.Bookings = null;
                    item.ItemsInRents = null;
                    item.Bookings = null;
                    item.Maintenances = null;
                    return this.Ok(item);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Item item)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Item newItem = new Item
                    {
                        Name = item.Name,
                        Info = item.Info,
                        URLphoto = item.URLphoto,
                        CostPerHour = item.CostPerHour,
                        Status = item.Status,
                        Cost = item.Cost,
                        Size = item.Size
                    };

                    await this.repository.AddAsync<Item>(newItem);

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
        public async Task<IActionResult> Update([FromBody] Item item)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Item oldItem = await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId);
                    if (oldItem == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Item not found." });
                    }

                    oldItem.Name = item.Name;
                    oldItem.Info = item.Info;
                    oldItem.URLphoto = item.URLphoto;
                    oldItem.CostPerHour = item.CostPerHour;
                    oldItem.Status = item.Status;
                    oldItem.Cost = item.Cost;
                    oldItem.Size = item.Size;

                    await this.repository.UpdateAsync<Item>(oldItem);
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
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Item item = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
                    await this.repository.DeleteAsync<Item>(item);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        public string TokenFromHeader(HttpRequest request)
        {
            var re = Request;
            var headers = re.Headers;
            string token = "";
            if (headers.ContainsKey("token"))
            {
                token = headers["token"];
            }
            return token;
        }
    }
}
