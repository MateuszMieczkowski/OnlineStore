using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Server.Entities.Configurations
{
    public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
    {
        public void Configure(EntityTypeBuilder<TaxRate> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(e => e.Amount)
                .IsRequired();
        }
    }
}
