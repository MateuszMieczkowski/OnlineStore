using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Server.Entities.Configurations;

public class CounterConfiguration : IEntityTypeConfiguration<Counter>
{
    public void Configure(EntityTypeBuilder<Counter> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
