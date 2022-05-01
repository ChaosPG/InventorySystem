using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Areas.Identity.Data;
using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Models
{
    public class InventorySystemContext : IdentityDbContext<InventorySystemUser>
    {
        public InventorySystemContext(DbContextOptions<InventorySystemContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Order_Detail> Order_Details { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Payment>().ToTable("Payment");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Customer>().ToTable("Customer");
            builder.Entity<Supplier>().ToTable("Supplier");
            builder.Entity<Orders>().ToTable("Orders");
            builder.Entity<Order_Detail>().ToTable("Order_Detail");
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
