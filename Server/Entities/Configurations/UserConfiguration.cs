using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.OwnsOne(
            e => e.Preferences,
            b =>
            {
                b.Property(e => e.UITheme)
                    .IsRequired();

                b.Property(e => e.DisplayedPrice)
                    .IsRequired();

                b.Property(e => e.IsSubscribedToNewsLetter)
                    .IsRequired();

                b.Property(e => e.PageSize)
                    .IsRequired()
                    .HasDefaultValue(20);
            });

        builder.HasMany(e => e.Orders)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}