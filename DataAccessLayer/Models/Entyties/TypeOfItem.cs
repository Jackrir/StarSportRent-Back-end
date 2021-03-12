using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class TypeOfItem : BaseTable
    {
        [Key]

        public int TypeId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public List<Item> Items { get; set; }
    }
}
