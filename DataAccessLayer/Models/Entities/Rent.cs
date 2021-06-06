using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class Rent : BaseTable
    {
        [Key]
        public int RentId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public string Status { get; set; }


        public List<ItemsInRent> ItemsInRents { get; set; }
    }
}
