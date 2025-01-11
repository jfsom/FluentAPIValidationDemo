using Microsoft.EntityFrameworkCore;

namespace FluentAPIValidationDemo.Models
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed some initial data using the Fluent API
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Laptop", Price = 750.00m, Stock = 20, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Smartphone", Price = 500.00m, Stock = 50, CategoryId = 2 },
                new Product { ProductId = 3, Name = "Headphones", Price = 100.00m, Stock = 100, CategoryId = 3 }
            );
        }
        public DbSet<Product> Products { get; set; }
    }
}
