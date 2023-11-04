using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Server.Entities.Configurations
{
    public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreferences>
    {
        public void Configure(EntityTypeBuilder<UserPreferences> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.UITheme)
                .IsRequired();

            builder.Property(e => e.DisplayedPrice)
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<UserPreferences>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
