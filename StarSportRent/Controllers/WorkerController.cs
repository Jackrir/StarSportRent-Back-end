using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IRentCalculate rentCalculate;
        private readonly IWorkerRent worker;

        public WorkerController(IRepository repository,IRentCalculate rentCalculate, IWorkerRent worker)
        {
            this.repository = repository;
            this.rentCalculate = rentCalculate;
            this.worker = worker;
        }

        [HttpGet]
        public async Task<IActionResult> GetRentInRent()
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    IEnumerable<Rent> rent = await this.repository.GetRangeAsync<Rent>(true, x => x.Status == "Rent");
                    foreach (Rent item in rent)
                    {
                        item.ItemsInRents = null;
                        item.User = null;
                    }
                    return this.Ok(rent.ToArray());
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDelayRent()
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    IEnumerable<Rent> rent = await this.repository.GetRangeAsync<Rent>(true, x => x.Status == "Rent" && DateTime.UtcNow > x.FinishTime);
                    foreach (Rent item in rent)
                    {
                        item.ItemsInRents = null;
                        item.User = null;
                    }
                    return this.Ok(rent.ToArray());
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CalculateRent(int id)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    return Ok(new ErrorMessage { message = Convert.ToString(await this.rentCalculate.Calculate(id, this.repository)) });
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PayRent(int id)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    Rent oldRent = await this.repository.GetAsync<Rent>(true, x => x.RentId == id);
                    if (oldRent == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Rent not found." });
                    }

                    oldRent.Status = "Finish";
                    IEnumerable<ItemsInRent> items = await this.repository.GetRangeAsync<ItemsInRent>(true, x => x.RentId == oldRent.RentId);
                    foreach(ItemsInRent item in items)
                    {
                        Item element = await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId);
                        element.Status = "Ok";

                        await this.repository.UpdateAsync<Item>(element);
                    }

                    await this.repository.UpdateAsync<Rent>(oldRent);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemStorage()
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
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
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OkToMaintenance(int id)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    Item oldItem = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
                    if (oldItem == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Item not found." });
                    }

                    oldItem.Status = "Maintenance";
                    await this.repository.AddAsync<Maintenance>(new Maintenance { ItemId = oldItem.ItemId, StartTime = DateTime.UtcNow });

                    await this.repository.UpdateAsync<Item>(oldItem);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemMaintenance()
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    IEnumerable<Item> items = await this.repository.GetRangeAsync<Item>(true, x => x.Status == "Maintenance");

                    foreach (Item el in items)
                    {
                        el.Bookings = null;
                        el.ItemsInRents = null;
                        el.Bookings = null;
                        el.Maintenances = null;
                    }
                    return this.Ok(items.ToArray());
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> MaintenanceToOK(int id)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    Item oldItem = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
                    if (oldItem == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Item not found." });
                    }

                    oldItem.Status = "Ok";
                    Maintenance maintenance =  await this.repository.GetAsync<Maintenance>(true, x => x.ItemId == id && x.FinishTime == DateTime.MinValue);
                    maintenance.FinishTime = DateTime.UtcNow;

                    await this.repository.UpdateAsync<Maintenance>(maintenance);
                    await this.repository.UpdateAsync<Item>(oldItem);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBooking(int id)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    IEnumerable<Booking> bookings = await this.repository.GetRangeAsync<Booking>(true, x => x.UserId == id);
                    List<Item> items = new List<Item>();
                    foreach(Booking item in bookings)
                    {
                        Item element = await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId);
                        element.Bookings = null;
                        element.ItemsInRents = null;
                        element.Maintenances = null;
                        element.TypeItems = null;
                        items.Add(element);
                    }
                    return this.Ok(items);
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CostRentBooking([FromBody] RentBookingRequest request)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    return Ok(new ErrorMessage { message = Convert.ToString(await this.worker.CalculateBookingRent(request.id,request.time)) });
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SuccessUserBooking([FromBody] RentBookingRequest request)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    await this.worker.UserBooking(request.id,request.time);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRentCost([FromBody] RentRequest request)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    return Ok(new ErrorMessage { message = Convert.ToString(await this.worker.CalculateRentCost(request.data, request.time)) });
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRent([FromBody] CreateRentRequest request)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "worker")
                {
                    await this.worker.CreateRent(request.data, request.time, request.id);
                    return this.Ok();
                }
                return this.NotFound(new ErrorMessage { message = "No worker" });
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
