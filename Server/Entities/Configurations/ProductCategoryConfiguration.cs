using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(e => e.Products)
            .WithOne(p => p.ProductCategory)
            .HasForeignKey(p => p.ProductCategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}