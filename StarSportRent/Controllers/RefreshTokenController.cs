using BusinessLogicLayer.Models;
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
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRepository repository;

        public RefreshTokenController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Refresh()
        {
            var re = Request;
            var headers = re.Headers;
            string token = "";
            if (headers.ContainsKey("token"))
            {
                token = headers["token"];
            }
            AuthService service = new AuthService(repository);
            var (checktoken, tokens) = await service.RefreshToken(token);
            if(checktoken)
            {
                return this.Ok(tokens);
            } 
            else
            {
                return this.NotFound(new ErrorMessage { message = "Invalit RefreshToken"});
            }
        }
    }
}
