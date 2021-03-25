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
    public class BookingController : ControllerBase
    {
        private readonly IRepository repository;

        public BookingController(IRepository repository)
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
                    IEnumerable<Booking> booking = await this.repository.GetRangeAsync<Booking>(true, x => true);
                    foreach (Booking el in booking)
                    {
                        el.Item = null;
                        el.User = null;
                    }
                    return this.Ok(booking.ToArray());
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
                if (role == "admin")
                {
                    Booking booking = await this.repository.GetAsync<Booking>(true, x => x.BookingId == id);
                    if (booking == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Booking not found." });
                    }
                    booking.Item = null;
                    booking.User = null;
                    return Ok(booking);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Booking booking)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Booking newBooking = new Booking
                    {
                        UserId = booking.UserId,
                        ItemId = booking.ItemId,
                        FinishBooking = booking.FinishBooking
                    };

                    await this.repository.AddAsync<Booking>(newBooking);

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
        public async Task<IActionResult> Update([FromBody] Booking booking)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Booking oldBooking = await this.repository.GetAsync<Booking>(true, x => x.BookingId == booking.BookingId);
                    if (oldBooking == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Booking not found." });
                    }

                    oldBooking.UserId = booking.UserId;
                    oldBooking.ItemId = booking.ItemId;
                    oldBooking.FinishBooking = booking.FinishBooking;

                    await this.repository.UpdateAsync<Booking>(oldBooking);
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
                    Booking booking = await this.repository.GetAsync<Booking>(true, x => x.BookingId == id);
                    await this.repository.DeleteAsync<Booking>(booking);
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
