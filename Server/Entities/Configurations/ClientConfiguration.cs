using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasBaseType<User>();

            builder.HasMany(e => e.Orders)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
