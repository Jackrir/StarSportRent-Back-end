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
    public class RentController : ControllerBase
    {
        private readonly IRepository repository;

        public RentController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Rent>> Get()
        {
            IEnumerable<Rent> rent = await this.repository.GetRangeAsync<Rent>(true, x => true);
            return rent.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<Rent> GetId(int id)
        {
            Rent rent = await this.repository.GetAsync<Rent>(true, x => x.RentId == id);
            if (rent == null)
            {
                throw new Exception("Rent not found.");
            }
            return rent;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Rent rent)
        {

            Rent newRent = new Rent
            {
                UserId = rent.UserId,
                StartTime = DateTime.UtcNow,
                FinishTime = rent.FinishTime,
                Status = "Rent"
            };

            await this.repository.AddAsync<Rent>(newRent);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Rent rent)
        {
            Rent oldRent = await this.repository.GetAsync<Rent>(true, x => x.RentId == rent.RentId);
            if (oldRent == null)
            {
                throw new Exception("Rent not found.");
            }

            oldRent.UserId = rent.UserId;
            oldRent.StartTime = rent.StartTime;
            oldRent.FinishTime = rent.FinishTime;
            oldRent.Status = rent.Status;

            await this.repository.UpdateAsync<Rent>(oldRent);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Rent rent = await this.repository.GetAsync<Rent>(true, x => x.RentId == id);
            await this.repository.DeleteAsync<Rent>(rent);
            return this.Ok();
        }
    }
}
