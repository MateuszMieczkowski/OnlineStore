using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Server.Entities.Configurations
{
    public class OrderAddressConfiguration : IEntityTypeConfiguration<OrderAddress>
    {
        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.StreetNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.State)
                .HasMaxLength(100);

            builder.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
