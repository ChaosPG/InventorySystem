using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Orders
    {
        [Key]
        public int Order_ID { get; set; }
        [DisplayName("วันที่สั่งซื้อ")]
        [Required(ErrorMessage = "กรุณาใส่ วันที่สั่งซื้อ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date_of_Order { get; set; }
        [DisplayName("รายละเอียดการสั่งซื้อ")]
        public string Order_Details { get; set; }
        [DisplayName("หมายเลขลูกค้า")]
        public int Customer_ID { get; set; }

        [ForeignKey("Customer_ID")]
        public virtual Customer Customer { get; set; }
    }
}
