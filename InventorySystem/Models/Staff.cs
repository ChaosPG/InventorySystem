using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Staff
    {
        [Key]
        public int Staff_ID { get; set; }
        public string Staff_FirstName { get; set; }
        public string Staff_LastName { get; set; }
        public string Staff_Address { get; set; }
        public string Staff_Phone { get; set; }
        public string Staff_Email { get; set; }
        public string Staff_UserName { get; set; }
        public string Staff_Password { get; set; }
        public int Role_ID { get; set; }

        [ForeignKey("Role_ID")]
        public virtual Role Role {get;set;}

    }
}
