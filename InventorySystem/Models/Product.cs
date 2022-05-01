using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Product
    {
        [Key]
        public int Product_ID { get; set; }
        [DisplayName("ชื่อสินค้า")]
        public string Product_Name { get; set; }
        [DisplayName("รายละเอียดสินค้า")]
        public string Product_Description { get; set; }
        [DisplayName("หน่วยของสินค้า")]
        public string Product_Unit { get; set; }
        [DisplayName("ราคาสินค้า")]
        [Column(TypeName = "decimal(16,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Product_Price { get; set; }
        [DisplayName("จำนวนสินค้า")]
        public int Product_Quantity { get; set; }
        [DisplayName("รายละเอียดอื่นๆ")]
        public string Other_Details { get; set; }
        [DisplayName("หมายเลขผู้ค้า")]
        public int Supplier_ID { get; set; }

        [ForeignKey("Supplier_ID")]
        public virtual Supplier Supplier { get; set; }
        [DisplayName("หมายเลขหมวดหมู่")]
        public int Category_ID { get; set; }
        [ForeignKey("Category_ID")]
        public virtual Category Category { get; set; }
    }
}
