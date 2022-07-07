using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace SneakersBase.Server
{
    public class SneakersDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }

        public SneakersDbContext(DbContextOptions<SneakersDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .Property(p => p.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Size>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(4);
                

            modelBuilder.Entity<ProductSize>()
                .Property(p => p.Quantity)
                .IsRequired();
        }
    }
}