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
    public class RentController : ControllerBase
    {
        private readonly IRepository repository;

        public RentController(IRepository repository)
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
                if (role == "admin" || role == "worker")
                {
                    IEnumerable<Rent> rent = await this.repository.GetRangeAsync<Rent>(true, x => true);
                    foreach (Rent item in rent)
                    {
                        item.ItemsInRents = null;
                        item.User = null;
                    }
                    return this.Ok(rent.ToArray());
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
                if (role == "admin" || role == "worker")
                {
                    Rent rent = await this.repository.GetAsync<Rent>(true, x => x.RentId == id);
                    if (rent == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Rent not found." });
                    }
                    rent.ItemsInRents = null;
                    rent.User = null;
                    return this.Ok(rent);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Rent rent)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Rent newRent = new Rent
                    {
                        UserId = rent.UserId,
                        StartTime = rent.StartTime,
                        FinishTime = rent.FinishTime,
                        Status = "Rent"
                    };

                    await this.repository.AddAsync<Rent>(newRent);

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
        public async Task<IActionResult> Update([FromBody] Rent rent)
        {
            string token = Taker.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Rent oldRent = await this.repository.GetAsync<Rent>(true, x => x.RentId == rent.RentId);
                    if (oldRent == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Rent not found." });
                    }

                    oldRent.UserId = rent.UserId;
                    oldRent.StartTime = rent.StartTime;
                    oldRent.FinishTime = rent.FinishTime;
                    oldRent.Status = rent.Status;

                    await this.repository.UpdateAsync<Rent>(oldRent);
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
                    Rent rent = await this.repository.GetAsync<Rent>(true, x => x.RentId == id);
                    await this.repository.DeleteAsync<Rent>(rent);
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
