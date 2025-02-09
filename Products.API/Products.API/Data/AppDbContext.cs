using Microsoft.EntityFrameworkCore;
using Products.API.Models;

namespace Products.API.Data
{
    public class AppDbContext(DbContextOptions opts) : DbContext(opts)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Description)
                .HasDatabaseName("IX_Product_Description");
        }
    }
}
