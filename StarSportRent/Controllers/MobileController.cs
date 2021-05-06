using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.API;
using PresentationLayer.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IMobileFunctions mobileFunction;
        public MobileController(IRepository repository, IMobileFunctions mobileFunction)
        {
            this.repository = repository;
            this.mobileFunction = mobileFunction;
        }

        [HttpGet]
        public async Task<IActionResult> ItemsOk()
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                IEnumerable<Item> items = await this.repository.GetRangeAsync<Item>(true, x => x.Status == "Ok");

                foreach (Item el in items)
                {
                    el.Bookings = null;
                    el.ItemsInRents = null;
                    el.Bookings = null;
                    el.Maintenances = null;
                }
                return this.Ok(items.ToArray());
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> ItemsOk(string search)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                IEnumerable<Item> items = await this.repository.GetRangeAsync<Item>(true, x => x.Status == "Ok" && x.Name.Contains(search));

                foreach (Item el in items)
                {
                    el.Bookings = null;
                    el.ItemsInRents = null;
                    el.Bookings = null;
                    el.Maintenances = null;
                }
                return this.Ok(items.ToArray());
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooking()
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                User user = await service.GetUser(token);
                IEnumerable<Booking> booking = await this.repository.GetRangeAsync<Booking>(true, x => x.UserId == user.UserId);
                List<Item> items = new List<Item>();
                foreach (Booking item in booking)
                {
                    items.Add(await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId));
                }
                foreach (Item el in items)
                {
                    el.Bookings = null;
                    el.ItemsInRents = null;
                    el.Bookings = null;
                    el.Maintenances = null;
                }
                return this.Ok(items.ToArray());
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                User user = await service.GetUser(token);
                Booking booking = await this.repository.GetAsync<Booking>(true, x => x.UserId == user.UserId && x.ItemId == id);
                await this.repository.DeleteAsync<Booking>(booking);
                Item item = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
                item.Status = "Ok";
                await this.repository.UpdateAsync<Item>(item);
                return this.Ok();
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRent()
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                User user = await service.GetUser(token);
                IEnumerable<Rent> rents = await this.repository.GetRangeAsync<Rent>(true, x => x.UserId == user.UserId);
                foreach (Rent el in rents)
                {
                    el.User = null;
                    el.ItemsInRents = null;
                }
                return this.Ok(rents.ToArray());
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                Item item = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
                item.ItemsInRents = null;
                item.Maintenances = null;
                item.TypeItems = null;
                item.Bookings = null;
                return this.Ok(item);
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody]CreateBookingRequest data)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                User user = await service.GetUser(token);
                DateTime date = DateTime.UtcNow.AddDays(data.count);
                bool result = await this.mobileFunction.CreateBooking(
                    new Booking() { UserId = user.UserId, ItemId = data.id, FinishBooking = date}, 
                    this.repository
                    );
                if (result)
                {
                    return this.Ok();
                }
                else
                {
                    return this.NotFound(new ErrorMessage { message = "Booking error" });
                }
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ItemHistory>> GetHistory(int id)
        {
            return await this.mobileFunction.GetItemHistory(id, this.repository);
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetСategory(int id)
        {
            return await this.repository.GetRangeAsync<Category>(true, x => true);
        }


        [HttpGet("{id}")]
        public async Task<Item[]> GetSetByCategory(int id)
        {
            return await this.mobileFunction.GetSetByCategory(id, this.repository);
        }

        [HttpGet("{itemId}/{categoryId}")]
        public async Task<IEnumerable<Item>> GetItemType(int itemId, int categoryId)
        {
            return await this.mobileFunction.GetItemType(itemId,categoryId, repository);
        }
    }
}
