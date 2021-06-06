using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class MobileFunctions : IMobileFunctions
    {

        public async Task<IEnumerable<ItemHistory>> GetItemHistory(int id, IRepository repository)
        {
            IEnumerable<ItemsInRent> itemsInRent = await repository.GetRangeAsync<ItemsInRent>(true, x => x.ItemId == id);
            List<Rent> rents = new List<Rent>();
            foreach(ItemsInRent item in itemsInRent)
            {
                rents.Add(await repository.GetAsync<Rent>(true, x => x.RentId == item.RentId));
            }

            IEnumerable<Maintenance> maintenances = await repository.GetRangeAsync<Maintenance>(true, x => x.ItemId == id);
            if(rents.Count > 0 && maintenances != null)
            {
                return GetHistoryFromTwoList(rents, ConvertToListMaintenance(maintenances));
            }
            else if (rents.Count == 0 && maintenances != null)
            {
                return GetHistoryFromMaintenance(ConvertToListMaintenance(maintenances));
            }
            else if (rents.Count > 0 && maintenances == null)
            {
                return GetHistoryFromRent(rents);
            }
            else
            {
                List<ItemHistory> result = new List<ItemHistory>();
                result.Add(new ItemHistory { start = DateTime.UtcNow, finish = DateTime.UtcNow, Status = "New" });
                return result;
            }

        }

        private IEnumerable<ItemHistory> GetHistoryFromRent(List<Rent> rents)
        {
            List<Rent> sortRents = rents.OrderBy(x => x.StartTime).ToList();
            Rent[] rentArray = sortRents.ToArray();
            int rentIndex = 0;
            int state = 0;
            DateTime temp = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            List<ItemHistory> result = new List<ItemHistory>();
            while (true)
            {
                if (rentIndex < rentArray.Length)
                {
                    temp = rentArray[rentIndex].StartTime;
                    state = 1;
                }

                if ((temp - end).TotalDays > 1 && end != DateTime.MinValue)
                {
                    result.Add(new ItemHistory { start = end, finish = temp, Status = "Storage" });
                }

                if (state == 1)
                {
                    result.Add(new ItemHistory { start = rentArray[rentIndex].StartTime, finish = rentArray[rentIndex].FinishTime, Status = "Rent" });
                    end = rentArray[rentIndex].FinishTime;
                    rentIndex++;
                    state = 0;
                    temp = DateTime.MinValue;
                    
                }
                else
                {
                    if ((DateTime.UtcNow - end).TotalDays > 1)
                    {
                        result.Add(new ItemHistory { start = end, finish = DateTime.UtcNow, Status = "Storage" });
                    }
                    break;
                }
            }
            return result;
        }
        private IEnumerable<ItemHistory> GetHistoryFromMaintenance(List<Maintenance> maintenances)
        {
            List<Maintenance> sortMaintenances = maintenances.OrderBy(x => x.StartTime).ToList();
            Maintenance[] maintenanceArray = sortMaintenances.ToArray();
            int maintenanceIndex = 0;
            int state = 0;
            DateTime temp = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            List<ItemHistory> result = new List<ItemHistory>();
            while (true)
            {
                if (maintenanceIndex < maintenanceArray.Length)
                {
                    temp = maintenanceArray[maintenanceIndex].StartTime;
                    state = 1;
                }

                if ((temp - end).TotalDays > 1 && end != DateTime.MinValue)
                {
                    result.Add(new ItemHistory { start = end, finish = temp, Status = "Storage" });
                }

                if (state == 1)
                {
                    result.Add(new ItemHistory { start = maintenanceArray[maintenanceIndex].StartTime, finish = maintenanceArray[maintenanceIndex].FinishTime, Status = "Maintenance" });
                    end = maintenanceArray[maintenanceIndex].FinishTime;
                    maintenanceIndex++;
                    state = 0;
                    temp = DateTime.MinValue;
                    
                }
                else
                {
                    if ((DateTime.UtcNow - end).TotalDays > 1)
                    {
                        result.Add(new ItemHistory { start = end, finish = DateTime.UtcNow, Status = "Storage" });
                    }
                    break;
                }
            }
            return result;
        }

        private IEnumerable<ItemHistory> GetHistoryFromTwoList(List<Rent> rents, List<Maintenance> maintenances)
        {
            List<Rent> sortRents = rents.OrderBy(x => x.StartTime).ToList();
            List<Maintenance> sortMaintenances = maintenances.OrderBy(x => x.StartTime).ToList();
            Rent[] rentArray = sortRents.ToArray();
            int rentIndex = 0;
            Maintenance[] maintenanceArray = sortMaintenances.ToArray();
            int maintenanceIndex = 0;
            int state = 0;
            DateTime temp = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            List<ItemHistory> result = new List<ItemHistory>();
            while (true)
            {
                if(rentIndex < rentArray.Length)
                {
                    temp = rentArray[rentIndex].StartTime;
                    state = 1;
                }
                if(maintenanceIndex < maintenanceArray.Length && state == 0)
                {
                    temp = maintenanceArray[maintenanceIndex].StartTime;
                    state = 2;
                }
                else if(maintenanceIndex < maintenanceArray.Length && state == 1)
                {
                    if(temp > maintenanceArray[maintenanceIndex].StartTime)
                    {
                        temp = maintenanceArray[maintenanceIndex].StartTime;
                        state = 2;
                    }
                }

                if((temp - end).TotalDays > 1 && end != DateTime.MinValue)
                {
                    result.Add(new ItemHistory { start = end, finish = temp, Status = "Storage" });
                }

                if(state == 1)
                {
                    result.Add(new ItemHistory { start = rentArray[rentIndex].StartTime, finish = rentArray[rentIndex].FinishTime, Status = "Rent" });
                    end = rentArray[rentIndex].FinishTime;
                    rentIndex++;
                    state = 0;
                    temp = DateTime.MinValue;
                    
                }
                else if (state == 2)
                {
                    result.Add(new ItemHistory { start = maintenanceArray[maintenanceIndex].StartTime, finish = maintenanceArray[maintenanceIndex].FinishTime, Status = "Maintenance" });
                    end = maintenanceArray[maintenanceIndex].FinishTime;
                    maintenanceIndex++;
                    state = 0;
                    temp = DateTime.MinValue;
                }
                else
                {
                    if((DateTime.UtcNow - end).TotalDays > 1)
                    {
                        result.Add(new ItemHistory { start = end, finish = DateTime.UtcNow, Status = "Storage" });
                    }
                    break;
                }
            }
            return result;
        }

        private List<Maintenance> ConvertToListMaintenance(IEnumerable<Maintenance> list)
        {
            List<Maintenance> maintenances = new List<Maintenance>();
            foreach(Maintenance item in list)
            {
                maintenances.Add(item);
            }
            return maintenances;
        }

        public async Task<bool> CreateBooking(Booking booking, IRepository repository)
        {
            Item item = await repository.GetAsync<Item>(true, x => x.ItemId == booking.ItemId);
            if(item.Status == "Ok")
            {
                item.Status = "Booking";
                await repository.UpdateAsync<Item>(item);
                Booking newBooking = new Booking
                {
                    UserId = booking.UserId,
                    ItemId = booking.ItemId,
                    FinishBooking = booking.FinishBooking
                };
                await repository.AddAsync<Booking>(newBooking);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Item[]> GetSetByCategory(int categoryId, IRepository repository)
        {
            List<Item> result = new List<Item>();
            IEnumerable<TypeOfItem> types = await repository.GetRangeAsync<TypeOfItem>(true, x => x.CategoryId == categoryId);

            foreach(TypeOfItem item in types)
            {
                foreach(TypeItem element in await repository.GetRangeAsync<TypeItem>(true, x => x.TypeId == item.TypeId))
                {
                    Item currentItem = await repository.GetAsync<Item>(true, x => x.ItemId == element.ItemId);
                    if(currentItem.Status == "Ok")
                    {
                        result.Add(ItemDeletRef(currentItem));
                        break;
                    }
                }
            }
            return result.ToArray();
        }

        public async Task<Item[]> GetItemType(int itemId, int categoryId, IRepository repository)
        {
            IEnumerable<TypeOfItem> types = await repository.GetRangeAsync<TypeOfItem>(true, x => x.CategoryId == categoryId);
            int needType = 0;
            foreach(TypeOfItem element in types)
            {
                TypeItem t = await repository.GetAsync<TypeItem>(true, x => x.ItemId == itemId && x.TypeId == element.TypeId);
                if(t != null)
                {
                    needType = t.TypeId;
                    break;
                }
            }
            IEnumerable<TypeItem> typeItems = await repository.GetRangeAsync<TypeItem>(true, x => x.TypeId == needType);
            List<Item> items = new List<Item>();
            foreach(TypeItem element in typeItems)
            {
                Item t = await repository.GetAsync<Item>(true, x => x.ItemId == element.ItemId && x.ItemId != itemId && x.Status == "Ok");
                if(t != null)
                {
                    t.Maintenances = null;
                    t.Bookings = null;
                    t.ItemsInRents = null;
                    t.TypeItems = null;
                    items.Add(t);
                }
                
            }
            return items.ToArray();
        }

        private Item ItemDeletRef(Item item)
        {
            item.Bookings = null;
            item.Maintenances = null;
            item.ItemsInRents = null;
            item.TypeItems = null;
            return item;
        }
    }
}
