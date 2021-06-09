using BusinessLogicLayer.API.Requests;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        private readonly IHubContext<IoTHub, IIoTHub> _chatHub;

        public IoTController(IHubContext<IoTHub, IIoTHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpPost]
        public async Task Post([FromBody]IoTMessage message)
        {
            await _chatHub.Clients.All.ReceiveMessage(message);
        }
    }
}
