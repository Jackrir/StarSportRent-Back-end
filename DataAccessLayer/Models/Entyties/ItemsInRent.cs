using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class ItemsInRent : BaseTable
    {
        [Key]
        public int RentId { get; set; }
        public Rent Rent { get; set; }
        [Key]
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
