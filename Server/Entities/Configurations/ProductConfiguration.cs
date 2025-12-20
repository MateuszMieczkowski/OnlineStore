using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace OnlineStore.Server.Entities.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.ShortDescription)
            .HasMaxLength(500);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.PriceNet)
            .IsRequired();
        
        builder.Property(e => e.PriceGross)
            .IsRequired();

        builder.Property(e => e.IsHidden)
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .IsRequired();

        builder.Property(e => e.Quantity)
            .IsRequired();
        
        builder.Property(e => e.ThumbnailBlobUri)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(e => e.TaxRate)
            .WithMany()
            .HasForeignKey(e => e.TaxRateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.ProductFiles)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<ProductFile>>(v, (JsonSerializerOptions?)null) ?? new List<ProductFile>()
            )
            .HasColumnType("nvarchar(max)");
    }
}