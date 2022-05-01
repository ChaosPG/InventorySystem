using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Payment
    {
        [Key]
        public int Bill_Number { get; set; }
        [DisplayName("ประเภทการชำระเงิน")]
        public string Payment_Type { get; set; }
        [DisplayName("รายละเอียดอื่นๆ")]
        public string Other_Details { get; set; }
    }
}
