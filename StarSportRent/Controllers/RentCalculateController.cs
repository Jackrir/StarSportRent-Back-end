using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
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
    public class RentCalculateController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IRentCalculate rentCalculate;
        public RentCalculateController (IRepository repository, IRentCalculate rentCalculate)
        {
            this.repository = repository;
            this.rentCalculate = rentCalculate;
        }

        [HttpGet("{id}")]
        public async Task<decimal> CalculateRent(int id)
        {
            return await this.rentCalculate.Calculate(id, this.repository);
        }
    }
}
