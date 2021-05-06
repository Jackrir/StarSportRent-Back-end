using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuth auth;

        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> LoginWeb(LoginRequest user)
        {
            var (result, code) = await this.auth.LoginAsyncWeb(user.login, user.password);
            if (result == null)
            {
                return this.NotFound(new ErrorMessage { message = code });
            }
            else
            {
                return this.Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest user)
        {
            var (result, code) = await this.auth.LoginAsync(user.login, user.password);
            if (result == null)
            {
                return this.NotFound(new ErrorMessage { message = code });
            }
            else
            {
                return this.Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequest user)
        {
            var (result, code) = await this.auth.RegistrationAsync(user.login, user.password, user.name);
            if (result == null)
            {
                return this.NotFound(new ErrorMessage { message = code });
            }
            else
            {
                return this.Ok(result);
            }
        }
    }
}
