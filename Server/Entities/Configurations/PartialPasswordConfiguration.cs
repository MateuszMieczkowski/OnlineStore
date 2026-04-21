using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class PartialPasswordConfiguration : IEntityTypeConfiguration<PartialPassword>
{
    public void Configure(EntityTypeBuilder<PartialPassword> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Fragment)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(e => e.StartPosition)
            .IsRequired();

        builder.Property(e => e.Length)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(e => e.PartialPasswords)
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
