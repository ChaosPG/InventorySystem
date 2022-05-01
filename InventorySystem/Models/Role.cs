using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Role
    {
        [Key]
        public int Role_ID { get; set; }
        public string Role_Name { get; set; }
        public string Role_Description { get; set; }
    }
}
