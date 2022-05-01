using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Order_Detail
    {
        [Key]
        public int Order_Detail_ID { get; set; }
        [DisplayName("ราคาต่อหน่วย")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Unit_Price { get; set; }
        [DisplayName("ขนาด")]
        public int Size { get; set; }
        [DisplayName("จำนวน")]
        public int Quantity { get; set; }
        [DisplayName("ส่วนลด")]
        public int Discount { get; set; }
        [DisplayName("ราคารวม")]
        [Column(TypeName = "decimal(16,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }
        [DisplayName("วันที่สั่งซื้อ")]
        [Required(ErrorMessage = "กรุณาใส่ วันที่สั่งซื้อ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Order_Detail_Date { get; set; }
        [DisplayName("สินค้า")]
        public int Product_ID { get; set; }

        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
        [DisplayName("รายละเอียดสินค้า")]
        public int Order_ID { get; set; }

        [ForeignKey("Order_ID")]
        public virtual Orders Orders { get; set; }

        [DisplayName("ประเภทการจ่ายเงิน")]
        public int Bill_Number { get; set; }

        [ForeignKey("Bill_Number")]
        public virtual Payment Payment { get; set; }
    }
}
