using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace OnlineStore.Server.Entities.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(e => e.TotalNet)
            .IsRequired();

        builder.Property(e => e.TotalGross)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Address)
            .WithMany()
            .HasForeignKey(e => e.OrderAddressId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.OrderItems)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<OrderItem>>(v, (JsonSerializerOptions?)null) ?? new List<OrderItem>()
            )
            .HasColumnType("nvarchar(max)");
    }
}