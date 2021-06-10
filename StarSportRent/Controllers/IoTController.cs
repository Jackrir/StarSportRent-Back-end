using BusinessLogicLayer.API.Requests;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
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
        private readonly IRepository repository;

        public IoTController(IHubContext<IoTHub, IIoTHub> chatHub, IRepository repository)
        {
            _chatHub = chatHub;
            this.repository = repository;
        }

        [HttpPost]
        public async Task Post([FromBody]IoTMessage message)
        {
            Item item = await repository.GetAsync<Item>(true, x => x.ItemId == Convert.ToInt32(message) && x.Status != "Rent");
            if(item != null)
            {
                await _chatHub.Clients.All.ReceiveMessage(message);
            }
        }
    }
}
