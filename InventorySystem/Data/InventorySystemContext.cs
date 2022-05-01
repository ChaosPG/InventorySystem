using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Models;

namespace InventorySystem.Data
{
    public class InventorySystemContext : DbContext
    {
        public InventorySystemContext (DbContextOptions<InventorySystemContext> options)
            : base(options)
        {
        }

        public DbSet<InventorySystem.Models.Category> Category { get; set; }

        public DbSet<InventorySystem.Models.Customer> Customer { get; set; }

        public DbSet<InventorySystem.Models.Order_Detail> Order_Detail { get; set; }

        public DbSet<InventorySystem.Models.Orders> Orders { get; set; }

        public DbSet<InventorySystem.Models.Payment> Payment { get; set; }

        public DbSet<InventorySystem.Models.Product> Product { get; set; }

        public DbSet<InventorySystem.Models.Supplier> Supplier { get; set; }
    }
}
