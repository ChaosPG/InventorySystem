using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Supplier
    {
        [Key]
        public int Supplier_ID { get; set; }
        [DisplayName("ชื่อบริษัทคู่ค้า")]
        public string Supplier_Name { get; set; }
        [DisplayName("ที่ตั้งบริษัทคู่ค้า")]
        public string Supplier_Address { get; set; }
        [DisplayName("หมายเลขโทรศัพท์")]
        public string Supplier_Phone { get; set; }
        [DisplayName("หมายเลขแฟกซ์")]
        public string Supplier_Fax { get; set; }
        [DisplayName("อีเมล์")]
        public string Supplier_Email { get; set; }
    }
}
