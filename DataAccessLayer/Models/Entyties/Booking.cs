using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class Booking : BaseTable
    {

        [Key]
        public int BookingId {get;set;}

        public int UserId { get; set; }

        public User User { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }

        public DateTime FinishBooking { get; set; }
    }
}
