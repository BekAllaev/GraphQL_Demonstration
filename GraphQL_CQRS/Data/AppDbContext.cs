using BogusWithInMemoryDb.Model;
using GraphQL_CQRS.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BogusWithInMemoryDb.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Optionally, override OnModelCreating to specify more configuration.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Category>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)        // A Product has one Category
                .WithMany(c => c.Products)      // A Category has many Products
                .HasForeignKey(p => p.CategoryId)  // Specify the foreign key property
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasMany(e => e.Orders)
                      .WithOne(o => o.Employee)
                      .HasForeignKey(o => o.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Customer ----------
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(c => c.CustomerId);

                entity.Property(c => c.CompanyName)
                      .IsRequired()
                      .HasMaxLength(100);

                // 1-ко-многим: Customer → Orders
                entity.HasMany(c => c.Orders)
                      .WithOne(o => o.Customer)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- Order ----------
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.OrderId);

                entity.Property(o => o.OrderDate)
                      .IsRequired();

                // связи к Employee и Customer уже настроены выше

                // 1-ко-многим: Order → OrderDetails
                entity.HasMany(o => o.OrderDetails)
                      .WithOne(od => od.Order)
                      .HasForeignKey(od => od.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- OrderDetail ----------
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");

                entity.HasKey(od => new { od.OrderId, od.ProductId });

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId);

                // FK → Product
                entity.HasOne(od => od.Product)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(od => od.ProductId);

                entity.Property(od => od.UnitPrice)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(od => od.Quantity)
                      .IsRequired();
            });
        }
    }
}
