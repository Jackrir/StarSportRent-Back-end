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
        public async Task<IEnumerable<Booking>> Get()
        {
            IEnumerable<Booking> booking = await this.repository.GetRangeAsync<Booking>(true, x => true);
            return booking.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<Booking> GetId(int id)
        {
            Booking booking = await this.repository.GetAsync<Booking>(true, x => x.BookingId == id);
            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }
            return booking;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Booking booking)
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Booking booking)
        {
            Booking oldBooking = await this.repository.GetAsync<Booking>(true, x => x.BookingId == booking.BookingId);
            if (oldBooking == null)
            {
                throw new Exception("Booking not found.");
            }

            oldBooking.UserId = booking.UserId;
            oldBooking.ItemId = booking.ItemId;
            oldBooking.FinishBooking = booking.FinishBooking;
            
            await this.repository.UpdateAsync<Booking>(oldBooking);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Booking booking = await this.repository.GetAsync<Booking>(true, x => x.BookingId == id);
            await this.repository.DeleteAsync<Booking>(booking);
            return this.Ok();
        }
    }
}
