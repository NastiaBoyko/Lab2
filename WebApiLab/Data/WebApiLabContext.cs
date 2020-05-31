using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiLab.Models;

namespace WebApiLab.Data
{
    public class WebApiLabContext : DbContext
    {
        public WebApiLabContext (DbContextOptions<WebApiLabContext> options)
            : base(options)
        {
        }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductOrder> ProductOrder { get; set; }
        public DbSet<WebApiLab.Models.Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Ignore(c => c.Products);
            modelBuilder.Entity<Product>().Ignore(c => c.Orders);
            modelBuilder.Entity<ProductOrder>().Ignore(c => c.Order);
            modelBuilder.Entity<ProductOrder>()
                .HasOne(pt => pt.Order)
                .WithMany(p => p.Products)
                .HasForeignKey(pt => pt.OrderId);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(pt => pt.Product)
                .WithMany(t => t.Orders)
                .HasForeignKey(pt => pt.ProductId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
