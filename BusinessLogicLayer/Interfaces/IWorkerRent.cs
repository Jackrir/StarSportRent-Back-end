using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IWorkerRent
    {
        Task<decimal> CalculateBookingRent(int id, DateTime time);
        Task<bool> UserBooking(int id, DateTime time);

        Task<decimal> CalculateRentCost(string data, DateTime time);

        Task<bool> CreateRent(string data, DateTime time, int id);
    }
}
