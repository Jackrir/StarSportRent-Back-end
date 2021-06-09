using BusinessLogicLayer.API.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IIoTHub
    {
        Task ReceiveMessage(IoTMessage message);
    }
}
