using Microsoft.EntityFrameworkCore;
using SneakersBase.Server.Entities;

namespace SneakersBase.Server
{
    public class SneakersDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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
                .IsRequired();
                

            modelBuilder.Entity<ProductSize>()
                .Property(p => p.Quantity)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();
        }
    }
}