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
    public class ItemsInRentController : ControllerBase
    {
        private readonly IRepository repository;

        public ItemsInRentController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemsInRent>> Get()
        {
            IEnumerable<ItemsInRent> itemsInRent = await this.repository.GetRangeAsync<ItemsInRent>(true, x => true);
            return itemsInRent.ToArray();
        }

        [HttpGet("{idRent}/{idItem}")]
        public async Task<ItemsInRent> GetId(int idRent, int idItem)
        {
            ItemsInRent itemsInRent = await this.repository.GetAsync<ItemsInRent>(true, x => x.RentId == idRent && x.ItemId == idItem);
            if (itemsInRent == null)
            {
                throw new Exception("ItemsInRent not found.");
            }
            return itemsInRent;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemsInRent booking)
        {

            ItemsInRent newItemsInRent = new ItemsInRent
            {
                RentId = booking.RentId,
                ItemId = booking.ItemId
            };

            await this.repository.AddAsync<ItemsInRent>(newItemsInRent);

            return this.Ok();
        }

        [HttpDelete("{idRent}/{idItem}")]
        public async Task<IActionResult> Delete(int idRent, int idItem)
        {
            ItemsInRent itemsInRent = await this.repository.GetAsync<ItemsInRent>(true, x => x.RentId == idRent && x.ItemId == idItem);
            await this.repository.DeleteAsync<ItemsInRent>(itemsInRent);
            return this.Ok();
        }
    }
}
