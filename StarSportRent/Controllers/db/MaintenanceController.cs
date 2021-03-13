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
    public class MaintenanceController : ControllerBase
    {
        private readonly IRepository repository;

        public MaintenanceController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Maintenance>> Get()
        {
            IEnumerable<Maintenance> maintenance = await this.repository.GetRangeAsync<Maintenance>(true, x => true);
            return maintenance.ToArray();
        }

        [HttpGet("{id}")]
        public async Task<Maintenance> GetId(int id)
        {
            Maintenance maintenance = await this.repository.GetAsync<Maintenance>(true, x => x.MaintenanceId == id);
            if (maintenance == null)
            {
                throw new Exception("Maintenance not found.");
            }
            return maintenance;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Maintenance maintenance)
        {

            Maintenance newMaintenance = new Maintenance
            {
                ItemId = maintenance.ItemId,
                StartTime = maintenance.StartTime,
                FinishTime = maintenance.FinishTime
            };

            await this.repository.AddAsync<Maintenance>(newMaintenance);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Maintenance maintenance)
        {
            Maintenance oldMaintenance = await this.repository.GetAsync<Maintenance>(true, x => x.MaintenanceId == maintenance.MaintenanceId);
            if (oldMaintenance == null)
            {
                throw new Exception("Maintenance not found.");
            }

            oldMaintenance.ItemId = maintenance.ItemId;
            oldMaintenance.StartTime = maintenance.StartTime;
            oldMaintenance.FinishTime = maintenance.FinishTime;

            await this.repository.UpdateAsync<Maintenance>(oldMaintenance);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Maintenance maintenance = await this.repository.GetAsync<Maintenance>(true, x => x.MaintenanceId == id);
            await this.repository.DeleteAsync<Maintenance>(maintenance);
            return this.Ok();
        }
    }
}
