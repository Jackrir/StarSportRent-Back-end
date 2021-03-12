using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class User : BaseTable
    {
        [Key]
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }

        public Token Token { get; set; }

        public List<Rent> Rents { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
