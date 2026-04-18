using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class MessagePermissionConfiguration : IEntityTypeConfiguration<MessagePermission>
{
    public void Configure(EntityTypeBuilder<MessagePermission> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.MessageId)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.HasOne(e => e.Message)
            .WithMany(m => m.AllowedEditors)
            .HasForeignKey(e => e.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.MessageId, e.UserId })
            .IsUnique();
    }
}

