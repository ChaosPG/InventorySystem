using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Customer
    {
        [Key]
        public int Customer_ID { get; set; }
        [DisplayName("ชื่อ")]
        public string Customer_FirstName { get; set; }
        [DisplayName("นามสกุล")]
        public string Customer_LastName { get; set; }
        [DisplayName("ที่อยู่")]
        public string Customer_Address { get; set; }
        [DisplayName("เบอร์โทรศัพท์")]
        public string Customer_Phone { get; set; }
        [DisplayName("อีเมล์")]
        public string Customer_Email { get; set; }
    }
}
