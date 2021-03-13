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
        public async Task<IEnumerable<Item>> Get()
        {
            IEnumerable<Item> item = await this.repository.GetRangeAsync<Item>(true, x => true);
            return item.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<Item> GetId(int id)
        {
            Item item = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
            if (item == null)
            {
                throw new Exception("Item not found.");
            }
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Item item)
        {

            Item newItem = new Item
            {
                TypeId = item.TypeId,
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Item item)
        {
            Item oldItem = await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId);
            if (oldItem == null)
            {
                throw new Exception("Item not found.");
            }

            oldItem.TypeId = item.TypeId;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Item item = await this.repository.GetAsync<Item>(true, x => x.ItemId == id);
            await this.repository.DeleteAsync<Item>(item);
            return this.Ok();
        }
    }
}
