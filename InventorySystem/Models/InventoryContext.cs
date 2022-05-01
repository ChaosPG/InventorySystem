using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Order_Detail> Order_Details { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Supplier>().ToTable("Supplier"); 
            modelBuilder.Entity<Orders>().ToTable("Orders");
            modelBuilder.Entity<Order_Detail>().ToTable("Order_Detail");
        }
    }
}
