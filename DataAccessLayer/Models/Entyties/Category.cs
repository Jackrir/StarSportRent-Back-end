using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class Category : BaseTable
    {
        [Key]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public List<TypeOfItem> TypeOfItems { get; set; }
    }
}
