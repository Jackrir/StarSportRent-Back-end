using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class Token : BaseTable
    {
        [Key]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string JWT { get; set; }

        public string RefreshToken { get; set; }

        public DateTime Time { get; set; }
    }
}
