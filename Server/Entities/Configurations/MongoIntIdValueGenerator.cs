using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace OnlineStore.Server.Entities.Configurations;

public class MongoIntIdValueGenerator : ValueGenerator<int>
{
    public override int Next(EntityEntry entry)
    {
        var entityName = entry.Entity.GetType().Name;
        var counter = entry.Context.Find<Counter>(entityName);
        if (counter is not null)
        {
            counter.Seq += 1;
            entry.Context.Update(counter);
        }
        else
        {
            counter = new Counter { Id = entityName, Seq = 1 };
            entry.Context.Add(counter);
            entry.Context.SaveChanges();
        }

        return counter.Seq;
    }

    public override bool GeneratesTemporaryValues => false;
}
