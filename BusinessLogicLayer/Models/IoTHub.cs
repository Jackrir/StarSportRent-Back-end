using BusinessLogicLayer.API.Requests;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class IoTHub : Hub<IIoTHub>
    {
    }
}
