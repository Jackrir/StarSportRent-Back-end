using BusinessLogicLayer.Interfaces;
using ClosedXML.Excel;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class ImportExport : IImportExport
    {
        private readonly IRepository repository;

        public ImportExport(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<byte[]> Export()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var workstheet = workbook.Worksheets.Add("Users");
                int currentRow = 1;
                workstheet.Cell(currentRow, 1).Value = "UserId";
                workstheet.Cell(currentRow, 2).Value = "Name";
                workstheet.Cell(currentRow, 3).Value = "Email";
                workstheet.Cell(currentRow, 4).Value = "Password";
                workstheet.Cell(currentRow, 5).Value = "Role";

                var users = await this.repository.GetRangeAsync<User>(true, x => true);

                foreach (var item in users)
                {
                    currentRow++;
                    workstheet.Cell(currentRow, 1).Value = item.UserId;
                    workstheet.Cell(currentRow, 2).Value = item.Name;
                    workstheet.Cell(currentRow, 3).Value = item.Email;
                    workstheet.Cell(currentRow, 4).Value = item.Password;
                    workstheet.Cell(currentRow, 5).Value = item.Role;
                }

                var workstheet1 = workbook.Worksheets.Add("Tokens");
                currentRow = 1;
                workstheet1.Cell(currentRow, 1).Value = "UserId";
                workstheet1.Cell(currentRow, 2).Value = "JWT";
                workstheet1.Cell(currentRow, 3).Value = "RefreshToken";
                workstheet1.Cell(currentRow, 4).Value = "Time";

                var tokens = await this.repository.GetRangeAsync<Token>(true, x => true);

                foreach (var item in tokens)
                {
                    currentRow++;
                    workstheet1.Cell(currentRow, 1).Value = item.UserId;
                    workstheet1.Cell(currentRow, 2).Value = item.JWT;
                    workstheet1.Cell(currentRow, 3).Value = item.RefreshToken;
                    workstheet1.Cell(currentRow, 4).Value = item.Time;
                }

                var workstheet2 = workbook.Worksheets.Add("Rents");
                currentRow = 1;
                workstheet2.Cell(currentRow, 1).Value = "RentId";
                workstheet2.Cell(currentRow, 2).Value = "UserId";
                workstheet2.Cell(currentRow, 3).Value = "StartTime";
                workstheet2.Cell(currentRow, 4).Value = "FinishTime";
                workstheet2.Cell(currentRow, 5).Value = "Status";

                var rents = await this.repository.GetRangeAsync<Rent>(true, x => true);

                foreach (var item in rents)
                {
                    currentRow++;
                    workstheet2.Cell(currentRow, 1).Value = item.RentId;
                    workstheet2.Cell(currentRow, 2).Value = item.UserId;
                    workstheet2.Cell(currentRow, 3).Value = item.StartTime;
                    workstheet2.Cell(currentRow, 4).Value = item.FinishTime;
                    workstheet2.Cell(currentRow, 5).Value = item.Status;
                }

                var workstheet3 = workbook.Worksheets.Add("Categories");
                currentRow = 1;
                workstheet3.Cell(currentRow, 1).Value = "CategoryId";
                workstheet3.Cell(currentRow, 2).Value = "Name";
                workstheet3.Cell(currentRow, 3).Value = "Info";

                var categories = await this.repository.GetRangeAsync<Category>(true, x => true);

                foreach (var item in categories)
                {
                    currentRow++;
                    workstheet3.Cell(currentRow, 1).Value = item.CategoryId;
                    workstheet3.Cell(currentRow, 2).Value = item.Name;
                    workstheet3.Cell(currentRow, 3).Value = item.Info;
                }

                var workstheet4 = workbook.Worksheets.Add("TypeOfItems");
                currentRow = 1;
                workstheet4.Cell(currentRow, 1).Value = "TypeId";
                workstheet4.Cell(currentRow, 2).Value = "CategoryId";
                workstheet4.Cell(currentRow, 3).Value = "Name";
                workstheet4.Cell(currentRow, 4).Value = "Info";

                var typeOfItems = await this.repository.GetRangeAsync<TypeOfItem>(true, x => true);

                foreach (var item in typeOfItems)
                {
                    currentRow++;
                    workstheet4.Cell(currentRow, 1).Value = item.TypeId;
                    workstheet4.Cell(currentRow, 2).Value = item.CategoryId;
                    workstheet4.Cell(currentRow, 3).Value = item.Name;
                    workstheet4.Cell(currentRow, 4).Value = item.Info;
                }

                var workstheet5 = workbook.Worksheets.Add("Items");
                currentRow = 1;
                workstheet5.Cell(currentRow, 1).Value = "ItemId";
                workstheet5.Cell(currentRow, 2).Value = "Name";
                workstheet5.Cell(currentRow, 3).Value = "URLphoto";
                workstheet5.Cell(currentRow, 4).Value = "Info";
                workstheet5.Cell(currentRow, 5).Value = "CostPerHour";
                workstheet5.Cell(currentRow, 6).Value = "Status";
                workstheet5.Cell(currentRow, 7).Value = "Cost";
                workstheet5.Cell(currentRow, 8).Value = "Size";

                var items = await this.repository.GetRangeAsync<Item>(true, x => true);

                foreach (var item in items)
                {
                    currentRow++;
                    workstheet5.Cell(currentRow, 1).Value = item.ItemId;
                    workstheet5.Cell(currentRow, 2).Value = item.Name;
                    workstheet5.Cell(currentRow, 3).Value = item.URLphoto;
                    workstheet5.Cell(currentRow, 4).Value = item.Info;
                    workstheet5.Cell(currentRow, 5).Value = item.CostPerHour;
                    workstheet5.Cell(currentRow, 6).Value = item.Status;
                    workstheet5.Cell(currentRow, 7).Value = item.Cost;
                    workstheet5.Cell(currentRow, 8).Value = item.Size;
                }

                var workstheet6 = workbook.Worksheets.Add("Maintenances");
                currentRow = 1;
                workstheet6.Cell(currentRow, 1).Value = "MaintenaceId";
                workstheet6.Cell(currentRow, 2).Value = "ItemId";
                workstheet6.Cell(currentRow, 3).Value = "StartTime";
                workstheet6.Cell(currentRow, 4).Value = "FinishTime";

                var maintenances = await this.repository.GetRangeAsync<Maintenance>(true, x => true);

                foreach (var item in maintenances)
                {
                    currentRow++;
                    workstheet6.Cell(currentRow, 1).Value = item.MaintenanceId;
                    workstheet6.Cell(currentRow, 2).Value = item.ItemId;
                    workstheet6.Cell(currentRow, 3).Value = item.StartTime;
                    workstheet6.Cell(currentRow, 4).Value = item.FinishTime;
                }

                var workstheet7 = workbook.Worksheets.Add("Bookings");
                currentRow = 1;
                workstheet7.Cell(currentRow, 1).Value = "BookingId";
                workstheet7.Cell(currentRow, 2).Value = "UserId";
                workstheet7.Cell(currentRow, 3).Value = "ItemId";
                workstheet7.Cell(currentRow, 4).Value = "FinishBooking";

                var bookings = await this.repository.GetRangeAsync<Booking>(true, x => true);

                foreach (var item in bookings)
                {
                    currentRow++;
                    workstheet7.Cell(currentRow, 1).Value = item.BookingId;
                    workstheet7.Cell(currentRow, 2).Value = item.UserId;
                    workstheet7.Cell(currentRow, 3).Value = item.ItemId;
                    workstheet7.Cell(currentRow, 4).Value = item.FinishBooking;
                }

                var workstheet8 = workbook.Worksheets.Add("TypeItems");
                currentRow = 1;
                workstheet8.Cell(currentRow, 1).Value = "ItemId";
                workstheet8.Cell(currentRow, 2).Value = "TypeId";

                var typeItems = await this.repository.GetRangeAsync<TypeItem>(true, x => true);

                foreach (var item in typeItems)
                {
                    currentRow++;
                    workstheet8.Cell(currentRow, 1).Value = item.ItemId;
                    workstheet8.Cell(currentRow, 2).Value = item.TypeId;
                }

                var workstheet9 = workbook.Worksheets.Add("ItemsInRents");
                currentRow = 1;
                workstheet9.Cell(currentRow, 1).Value = "RentId";
                workstheet9.Cell(currentRow, 2).Value = "ItemId";

                var itemsInRents = await this.repository.GetRangeAsync<ItemsInRent>(true, x => true);

                foreach (var item in itemsInRents)
                {
                    currentRow++;
                    workstheet9.Cell(currentRow, 1).Value = item.RentId;
                    workstheet9.Cell(currentRow, 2).Value = item.ItemId;
                }



                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }

            }
        }

        public async Task<bool> Import(string fName)
        {
            var fileName = $"{Directory.GetCurrentDirectory()}{@"\Content\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var (userMax, categoryMax, itemMax) = await GetMaxIdInDb();

                    Dictionary<int, int> userDictionary = new Dictionary<int, int>();
                    reader.Read();
                    while (reader.Read())
                    {
                        User newUser = await this.repository.AddAsync<User>(new User()
                        {
                            Name = reader.GetValue(1).ToString(),
                            Email = reader.GetValue(2).ToString(),
                            Password = reader.GetValue(3).ToString(),
                            Role = reader.GetValue(4).ToString()
                        }
                        );
                        userDictionary.Add(Convert.ToInt32(reader.GetValue(0).ToString()),newUser.UserId);
                    }
                    reader.NextResult();
                    reader.Read();
                    while (reader.Read())
                    {
                        Token oldToken = await this.repository.GetAsync<Token>(true, x => x.UserId == Convert.ToInt32(reader.GetValue(0).ToString()));
                        Token newRent = await this.repository.AddAsync<Token>(new Token()
                        {
                            UserId = userDictionary[Convert.ToInt32(reader.GetValue(0).ToString())],
                            JWT = oldToken?.JWT ?? "",
                            RefreshToken = oldToken?.RefreshToken ?? "",
                            Time = Convert.ToDateTime(reader.GetValue(3).ToString())
                        }
                        );
                    }

                    reader.NextResult();
                    Dictionary<int, int> rentDictionary = new Dictionary<int, int>();
                    reader.Read();
                    while (reader.Read())
                    {
                        Rent newRent = await this.repository.AddAsync<Rent>(new Rent()
                        {
                            UserId = userDictionary[Convert.ToInt32(reader.GetValue(1).ToString())],
                            StartTime = Convert.ToDateTime(reader.GetValue(2).ToString()),
                            FinishTime = Convert.ToDateTime(reader.GetValue(3).ToString()),
                            Status = reader.GetValue(4).ToString()
                        }
                        );
                        rentDictionary.Add(Convert.ToInt32(reader.GetValue(0).ToString()), newRent.RentId);
                    }

                    reader.NextResult();
                    Dictionary<int, int> categoryDictionary = new Dictionary<int, int>();
                    reader.Read();
                    while (reader.Read())
                    {
                        Category newCategory = await this.repository.AddAsync<Category>(new Category()
                        {
                            Name = reader.GetValue(1).ToString(),
                            Info = reader.GetValue(2)?.ToString() ?? ""
                        }
                        );
                        categoryDictionary.Add(Convert.ToInt32(reader.GetValue(0).ToString()), newCategory.CategoryId);
                    }

                    reader.NextResult();
                    Dictionary<int, int> typeDictionary = new Dictionary<int, int>();
                    reader.Read();
                    while (reader.Read())
                    {
                        TypeOfItem newTypeOfItem = await this.repository.AddAsync<TypeOfItem>(new TypeOfItem()
                        {
                            CategoryId = categoryDictionary[Convert.ToInt32(reader.GetValue(1).ToString())],
                            Name = reader.GetValue(2).ToString(),
                            Info = reader.GetValue(3)?.ToString() ?? ""
                        }
                        );
                        typeDictionary.Add(Convert.ToInt32(reader.GetValue(0).ToString()), newTypeOfItem.TypeId);
                    }

                    reader.NextResult();
                    Dictionary<int, int> itemDictionary = new Dictionary<int, int>();
                    reader.Read();
                    while (reader.Read())
                    {
                        Item newItem = await this.repository.AddAsync<Item>(new Item()
                        {
                            Name = reader.GetValue(1).ToString(),
                            URLphoto = reader.GetValue(2).ToString(),
                            Info = reader.GetValue(3)?.ToString() ?? "",
                            CostPerHour = Convert.ToInt32(reader.GetValue(4).ToString()),
                            Status = reader.GetValue(5).ToString(),
                            Cost = Convert.ToInt32(reader.GetValue(6).ToString()),
                            Size = reader.GetValue(7).ToString(),
                        }
                        );
                        itemDictionary.Add(Convert.ToInt32(reader.GetValue(0).ToString()), newItem.ItemId);
                    }

                    reader.NextResult();
                    reader.Read();
                    while (reader.Read())
                    {
                        Maintenance newMaintenance = await this.repository.AddAsync<Maintenance>(new Maintenance()
                        {
                            ItemId = itemDictionary[Convert.ToInt32(reader.GetValue(1).ToString())],
                            StartTime = Convert.ToDateTime(reader.GetValue(2).ToString()),
                            FinishTime = Convert.ToDateTime(reader.GetValue(3).ToString())
                        }
                        );
                    }

                    reader.NextResult();
                    reader.Read();
                    while (reader.Read())
                    {
                        Booking newBooking = await this.repository.AddAsync<Booking>(new Booking()
                        {
                            UserId = userDictionary[Convert.ToInt32(reader.GetValue(1).ToString())],
                            ItemId = itemDictionary[Convert.ToInt32(reader.GetValue(2).ToString())],
                            FinishBooking = Convert.ToDateTime(reader.GetValue(3).ToString())
                        }
                        );
                    }

                    reader.NextResult();
                    reader.Read();
                    while (reader.Read())
                    {
                        TypeItem newTypeItem = await this.repository.AddAsync<TypeItem>(new TypeItem()
                        {
                            ItemId = itemDictionary[Convert.ToInt32(reader.GetValue(0).ToString())],
                            TypeId = typeDictionary[Convert.ToInt32(reader.GetValue(1).ToString())]
                        }
                        );
                    }

                    reader.NextResult();
                    reader.Read();
                    while (reader.Read())
                    {
                        ItemsInRent newTypeItem = await this.repository.AddAsync<ItemsInRent>(new ItemsInRent()
                        {
                            RentId = rentDictionary[Convert.ToInt32(reader.GetValue(0).ToString())],
                            ItemId = itemDictionary[Convert.ToInt32(reader.GetValue(1).ToString())]
                        }
                        );
                    }

                    IEnumerable<User> users = await this.repository.GetRangeAsync<User>(true, x => x.UserId <= userMax);
                    await this.repository.DeleteRangeAsync<User>(users);

                    IEnumerable<Category> categories = await this.repository.GetRangeAsync<Category>(true, x => x.CategoryId <= categoryMax);
                    await this.repository.DeleteRangeAsync<Category>(categories);

                    IEnumerable<Item> items = await this.repository.GetRangeAsync<Item>(true, x => x.ItemId <= itemMax);
                    await this.repository.DeleteRangeAsync<Item>(items);

                }
            }
            return true;
        }

        private async Task<(int,int,int)> GetMaxIdInDb ()
        {
            IEnumerable<User> oldUser = await this.repository.GetRangeAsync<User>(true, x => true);
            int userMax = 0;
            foreach (User item in oldUser)
            {
                if (userMax < item.UserId)
                    userMax = item.UserId;
            }

            IEnumerable<Category> oldCategory = await this.repository.GetRangeAsync<Category>(true, x => true);
            int categoryMax = 0;
            foreach (Category item in oldCategory)
            {
                if (categoryMax < item.CategoryId)
                    categoryMax = item.CategoryId;
            }

            IEnumerable<Item> oldItem = await this.repository.GetRangeAsync<Item>(true, x => true);
            int itemMax = 0;
            foreach (Item item in oldItem)
            {
                if (itemMax < item.ItemId)
                    itemMax = item.ItemId;
            }
            return (userMax, categoryMax, itemMax);
        }
    }
}
