using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class WorkerRent : IWorkerRent
    {
        private readonly IRepository repository;

        public WorkerRent(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<decimal> CalculateBookingRent(int id, DateTime time)
        {
            IEnumerable<Booking> bookings = await this.repository.GetRangeAsync<Booking>(true, x => x.UserId == id);
            List<Item> items = new List<Item>();
            foreach (Booking item in bookings)
            {
                items.Add(await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId));
            }
            int hours = Convert.ToInt32((time - DateTime.UtcNow).TotalHours);
            decimal cost = 0;
            foreach (Item item in items)
            {
                cost += item.CostPerHour * (decimal)hours;
            }
            return cost;
        }


        public async Task<bool> UserBooking(int id, DateTime time)
        {
            IEnumerable<Booking> bookings = await this.repository.GetRangeAsync<Booking>(true, x => x.UserId == id);
            List<Item> items = new List<Item>();
            foreach (Booking item in bookings)
            {
                items.Add(await this.repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId));
            }
            Rent rent = await this.repository.AddAsync<Rent>(new Rent() { UserId = id, Status = "Rent", StartTime = DateTime.UtcNow, FinishTime = time});
            foreach (Item item in items)
            {
                await this.repository.AddAsync<ItemsInRent>(new ItemsInRent() { ItemId = item.ItemId, RentId = rent.RentId });
                item.Status = "Rent";
                await this.repository.UpdateAsync<Item>(item);
            }
            await this.repository.DeleteRangeAsync<Booking>(bookings);
            return true;
        }

        public async Task<decimal> CalculateRentCost(string data, DateTime time)
        {
            IEnumerable<Item> items = await GetItemsFromString(data);
            int hours = Convert.ToInt32((time - DateTime.UtcNow).TotalHours);
            decimal cost = 0;
            foreach (Item item in items)
            {
                cost += item.CostPerHour * (decimal)hours;
            }
            return cost;
        }

        private async Task<IEnumerable<Item>> GetItemsFromString(string data)
        {
            string[] splitString = data.Split(new char[] { ',' });
            List<Item> items = new List<Item>();
            foreach(string item in splitString)
            {
                items.Add(await this.repository.GetAsync<Item>(true, x => x.ItemId == Convert.ToInt32(item)));
            }
            return items;
        }

        public async Task<bool> CreateRent(string data, DateTime time, int id)
        {
            IEnumerable<Item> items = await GetItemsFromString(data);
            Rent rent = await this.repository.AddAsync<Rent>(new Rent() { UserId = id, Status = "Rent", StartTime = DateTime.UtcNow, FinishTime = time });
            foreach (Item item in items)
            {
                await this.repository.AddAsync<ItemsInRent>(new ItemsInRent() { ItemId = item.ItemId, RentId = rent.RentId });
                item.Status = "Rent";
                await this.repository.UpdateAsync<Item>(item);
            }
            return true;
        }
    }
}
