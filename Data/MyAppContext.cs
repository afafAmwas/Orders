using Microsoft.EntityFrameworkCore;
using Orders.Models;

namespace Orders.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }   
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FullName = "Masa" },
                new Customer { Id = 2, FullName = "Worood" },
                new Customer { Id = 3, FullName = "Sara" },
                new Customer { Id = 4, FullName = "Nidal" },
                new Customer { Id = 5, FullName = "Abdullah" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerId = 1, TotalAmount = 99.10m, CostAmount = 60.00m, OrderDate = new DateTime(2025, 1, 1) },
                new Order { Id = 2, CustomerId = 2, TotalAmount = 55.20m, CostAmount = 30.00m, OrderDate = new DateTime(2025, 1, 2) },
                new Order { Id = 3, CustomerId = 3, TotalAmount = 35.99m, CostAmount = 20.00m, OrderDate = new DateTime(2025, 1, 3) },
                new Order { Id = 4, CustomerId = 4, TotalAmount = 200.20m, CostAmount = 150.00m, OrderDate = new DateTime(2025, 1, 4) },
                new Order { Id = 5, CustomerId = 5, TotalAmount = 55.99m, CostAmount = 40.00m, OrderDate = new DateTime(2025, 1, 5) }
            );

        }
    }
}
