using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AuthorId)
            .IsRequired();

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.ModifiedDate)
            .IsRequired();

        builder.HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.AllowedEditors)
            .WithOne(p => p.Message)
            .HasForeignKey(p => p.MessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

