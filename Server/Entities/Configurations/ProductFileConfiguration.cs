using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Server.Entities.Configurations
{
    public class ProductFileConfiguration : IEntityTypeConfiguration<ProductFile>
    {
        public void Configure(EntityTypeBuilder<ProductFile> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.BlobUri)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(e => e.Product)
                .WithMany(p => p.ProductFiles)
                .HasForeignKey(e => e.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
