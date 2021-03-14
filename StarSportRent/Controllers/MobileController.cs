using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<IEnumerable<ItemHistory>> GetHistory(int id)
        {
            return await this.mobileFunction.GetItemHistory(id, this.repository);
        }

        [HttpPost]
        public async Task<IActionResult> Booking([FromBody] Booking booking)
        {
            bool result = await this.mobileFunction.CreateBooking(booking, this.repository);
            if (result)
            {
                return this.Ok();
            }
            else
            {
                throw new Exception("Booking error");
            }
        }

        [HttpGet("{id}")]
        public async Task<Item[]> GetSetByCategory(int id)
        {
            return await this.mobileFunction.GetSetByCategory(id, this.repository);
        }
    }
}
