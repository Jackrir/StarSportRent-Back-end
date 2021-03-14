using DataAccessLayer.Models.Entyties.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models.Entyties
{
    public class TypeItem : BaseTable
    {
        [Key]
        public int TypeId { get; set; }
        public TypeOfItem Type { get; set; }
        [Key]
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
