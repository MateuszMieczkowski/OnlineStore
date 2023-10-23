﻿using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;

namespace OnlineStore.Server;

public class OnlineStoreDbContext : DbContext
{
    public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<ProductSize> ProductSizes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(eb =>
        {
            eb.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            eb.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
        });


        modelBuilder.Entity<Product>()
            .Property(p => p.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

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