using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(e => e.PriceNet)
            .IsRequired();

        builder.Property(e => e.PriceGross)
            .IsRequired();

        builder.HasOne(e => e.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(e => e.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(e => e.Product, x =>
        {
            x.ToJson();
        });
    }
}