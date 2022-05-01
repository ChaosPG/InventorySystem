using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Category
    {
        [Key]
        public int Category_ID { get; set; }
        [DisplayName("ชื่อประเภท")]
        public string Category_Name { get; set; }
        [DisplayName("คำอธิบาย")]
        public string Category_Description { get; set; }

    }
}
