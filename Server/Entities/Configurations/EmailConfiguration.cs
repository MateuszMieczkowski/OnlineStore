using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(e => e.SenderEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.RecipientEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.HtmlContent)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.RecipientName)
            .IsRequired(false);

        builder.Property(e => e.AttemptCount)
            .HasDefaultValue(0);
    }
}