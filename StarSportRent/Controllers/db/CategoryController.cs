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
    public class CategoryController : ControllerBase
    {
        private readonly IRepository repository;

        public CategoryController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            IEnumerable<Category> category = await this.repository.GetRangeAsync<Category>(true, x => true);
            return category.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<Category> GetId(int id)
        {
            Category category = await this.repository.GetAsync<Category>(true, x => x.CategoryId == id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            return category;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {

            Category newCategory = new Category
            {
                Name = category.Name,
                Info = category.Info
            };

            await this.repository.AddAsync<Category>(newCategory);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Category category)
        {
            Category oldCategory = await this.repository.GetAsync<Category>(true, x => x.CategoryId == category.CategoryId);
            if (oldCategory == null)
            {
                throw new Exception("Category not found.");
            }

            oldCategory.Name = category.Name;
            oldCategory.Info = category.Info;

            await this.repository.UpdateAsync<Category>(oldCategory);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category rent = await this.repository.GetAsync<Category>(true, x => x.CategoryId == id);
            await this.repository.DeleteAsync<Category>(rent);
            return this.Ok();
        }
    }
}
