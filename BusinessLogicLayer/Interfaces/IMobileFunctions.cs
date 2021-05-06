using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IMobileFunctions
    {
        Task<IEnumerable<ItemHistory>> GetItemHistory(int id, IRepository repository);
        Task<bool> CreateBooking(Booking booking, IRepository repository);

        Task<Item[]> GetSetByCategory(int categoryId, IRepository repository);

        Task<Item[]> GetItemType(int itemId, int categoryId, IRepository repository);
    }
}
