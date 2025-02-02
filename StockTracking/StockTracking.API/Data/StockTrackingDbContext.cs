using Microsoft.EntityFrameworkCore;
using StockTracking.API.Models.Domain;

namespace StockTracking.API.Data
{
    public class StockTrackingDbContext : DbContext
    {
        public StockTrackingDbContext(DbContextOptions<StockTrackingDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSupply> ProductSupplies { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email is unique for the User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ProductSupply entity
            modelBuilder.Entity<ProductSupply>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity)
                      .IsRequired();
                entity.Property(e => e.Date)
                      .IsRequired();
                entity.Property(e => e.RemainingQuantity)
                      .IsRequired();
            });

            // ProductSale entity
            modelBuilder.Entity<ProductSale>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity)
                      .IsRequired();
                entity.Property(e => e.Date)
                      .IsRequired();
            });
            
            // Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                // product have many supplies, one-to-many relationship with cascade delete
                entity.HasMany(e => e.ProductSupplies)
                      .WithOne(ps => ps.Product)
                      .HasForeignKey(ps => ps.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                // product have many sales, one-to-many relationship with cascade delete
                entity.HasMany(e => e.ProductSales)
                      .WithOne(ps => ps.Product)
                      .HasForeignKey(ps => ps.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
