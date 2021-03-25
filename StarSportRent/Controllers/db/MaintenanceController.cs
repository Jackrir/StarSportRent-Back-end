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
    public class MaintenanceController : ControllerBase
    {
        private readonly IRepository repository;

        public MaintenanceController(IRepository repository)
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
                    IEnumerable<Maintenance> maintenance = await this.repository.GetRangeAsync<Maintenance>(true, x => true);
                    foreach (Maintenance el in maintenance)
                    {
                        el.Item = null;
                    }
                    return this.Ok(maintenance.ToArray());
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
                    Maintenance maintenance = await this.repository.GetAsync<Maintenance>(true, x => x.MaintenanceId == id);
                    if (maintenance == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Maintenance not found." });
                    }

                    maintenance.Item = null;
                    return this.Ok(maintenance);
                }
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Maintenance maintenance)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
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
                return this.NotFound(new ErrorMessage { message = "No admin" });
            }
            else
            {
                return this.NotFound(new ErrorMessage { message = "token died" });
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Maintenance maintenance)
        {
            string token = this.TokenFromHeader(Request);
            AuthService service = new AuthService(repository);
            var (checktoken, role) = await service.CheckToken(token);
            if (checktoken)
            {
                if (role == "admin")
                {
                    Maintenance oldMaintenance = await this.repository.GetAsync<Maintenance>(true, x => x.MaintenanceId == maintenance.MaintenanceId);
                    if (oldMaintenance == null)
                    {
                        return this.NotFound(new ErrorMessage { message = "Maintenance not found." });
                    }

                    oldMaintenance.ItemId = maintenance.ItemId;
                    oldMaintenance.StartTime = maintenance.StartTime;
                    oldMaintenance.FinishTime = maintenance.FinishTime;

                    await this.repository.UpdateAsync<Maintenance>(oldMaintenance);
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
                    Maintenance maintenance = await this.repository.GetAsync<Maintenance>(true, x => x.MaintenanceId == id);
                    await this.repository.DeleteAsync<Maintenance>(maintenance);
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
