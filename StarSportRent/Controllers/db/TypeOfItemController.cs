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
    public class TypeOfItemController : ControllerBase
    {
        private readonly IRepository repository;

        public TypeOfItemController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<TypeOfItem>> Get()
        {
            IEnumerable<TypeOfItem> type = await this.repository.GetRangeAsync<TypeOfItem>(true, x => true);
            return type.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<TypeOfItem> GetId(int id)
        {
            TypeOfItem type = await this.repository.GetAsync<TypeOfItem>(true, x => x.TypeId == id);
            if (type == null)
            {
                throw new Exception("Type not found.");
            }
            return type;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TypeOfItem type)
        {

            TypeOfItem newType = new TypeOfItem
            {
                CategoryId = type.CategoryId,
                Name = type.Name,
                Info = type.Info
            };

            await this.repository.AddAsync<TypeOfItem>(newType);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TypeOfItem type)
        {
            TypeOfItem oldType = await this.repository.GetAsync<TypeOfItem>(true, x => x.TypeId == type.TypeId);
            if (oldType == null)
            {
                throw new Exception("Type not found.");
            }

            oldType.CategoryId = type.CategoryId;
            oldType.Name = type.Name;
            oldType.Info = type.Info;

            await this.repository.UpdateAsync<TypeOfItem>(oldType);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            TypeOfItem type = await this.repository.GetAsync<TypeOfItem>(true, x => x.TypeId == id);
            await this.repository.DeleteAsync<TypeOfItem>(type);
            return this.Ok();
        }
    }
}
