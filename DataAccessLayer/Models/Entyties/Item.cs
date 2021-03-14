using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class Item : BaseTable
    {
        [Key]
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string URLphoto { get; set; }

        public string Info { get; set; }

        public decimal CostPerHour { get; set; }

        public string Status { get; set; }

        public decimal Cost { get; set; }

        public string Size { get; set; }


        public List<Maintenance> Maintenances { get; set; }

        public List<Booking> Bookings { get; set; }

        public List<ItemsInRent> ItemsInRents { get; set; }

        public List<TypeItem> TypeItems { get; set; }
    }
}
