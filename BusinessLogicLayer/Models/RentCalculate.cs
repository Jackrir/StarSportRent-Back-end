using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class RentCalculate : IRentCalculate
    {
        public async Task<decimal> Calculate(int rentId, IRepository repository)
        {
            IEnumerable<ItemsInRent> intemsInRent = await repository.GetRangeAsync<ItemsInRent>(true, x => x.RentId == rentId);

            List<Item> items = new List<Item>();
            foreach (ItemsInRent item in intemsInRent)
            {
                items.Add(await repository.GetAsync<Item>(true, x => x.ItemId == item.ItemId));
            }

            Rent rent = await repository.GetAsync<Rent>(true, x => x.RentId == rentId);
            int hours = Convert.ToInt32((DateTime.UtcNow - rent.StartTime).TotalHours);
            decimal cost = 0;
            foreach(Item item in items)
            {
                cost += item.CostPerHour * (decimal)hours;
            }
            return cost;
        }
    }
}
