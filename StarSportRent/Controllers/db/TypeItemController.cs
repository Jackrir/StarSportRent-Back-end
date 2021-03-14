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
    public class TypeItemController : ControllerBase
    {
        private readonly IRepository repository;

        public TypeItemController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<TypeItem>> Get()
        {
            IEnumerable<TypeItem> typeItem = await this.repository.GetRangeAsync<TypeItem>(true, x => true);
            return typeItem.ToArray();
        }

        [HttpGet("{idType}/{idItem}")]
        public async Task<TypeItem> GetId(int idType, int idItem)
        {
            TypeItem typeItem = await this.repository.GetAsync<TypeItem>(true, x => x.TypeId == idType && x.ItemId == idItem);
            if (typeItem == null)
            {
                throw new Exception("TypeItem not found.");
            }
            return typeItem;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TypeItem typeItem)
        {

            TypeItem newTypeItem = new TypeItem
            {
                TypeId = typeItem.TypeId,
                ItemId = typeItem.ItemId
            };

            await this.repository.AddAsync<TypeItem>(newTypeItem);

            return this.Ok();
        }

        [HttpDelete("{idType}/{idItem}")]
        public async Task<IActionResult> Delete(int idType, int idItem)
        {
            TypeItem typeItem = await this.repository.GetAsync<TypeItem>(true, x => x.TypeId == idType && x.ItemId == idItem);
            await this.repository.DeleteAsync<TypeItem>(typeItem);
            return this.Ok();
        }
    }
}
