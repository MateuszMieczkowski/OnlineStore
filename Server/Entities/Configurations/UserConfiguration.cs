using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Orders)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.ClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);
    }
}