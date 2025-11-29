using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<MongoIntIdValueGenerator>()
            .UseIdentityColumn();

        builder.Property(e => e.TotalNet)
            .IsRequired();

        builder.Property(e => e.TotalGross)
            .IsRequired();

        builder.Property(e => e.Status).IsRequired();

        builder.Property(e => e.ClientId)
            .IsRequired();

        builder.Property(e => e.OrderAddressId)
            .IsRequired();
        
        builder.Property(e => e.ClientEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.OwnsMany(x => x.OrderItems, b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<MongoIntIdValueGenerator>()
                .UseIdentityColumn();
            b.OwnsOne(e => e.Product, x => x.ToJson());
        });

        // builder.HasOne(e => e.Address)
        //     .WithMany()
        //     .HasForeignKey(e => e.OrderAddressId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.HasMany(e => e.OrderItems)
        //     .WithOne(item => item.Order)
        //     .HasForeignKey(item => item.OrderId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}