using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Server.Entities.Configurations
{
    public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.UITheme)
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<UserPreference>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
