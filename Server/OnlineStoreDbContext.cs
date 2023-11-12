using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Entities.Abstractions;
using OnlineStore.Server.Services;

namespace OnlineStore.Server;

public class OnlineStoreDbContext : DbContext
{
    private readonly IClock _clock;
    public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options, IClock clock) : base(options)
    {
        _clock = clock;
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public DbSet<ProductFile> ProductFiles { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrdersItems { get; set; } = null!;
    public DbSet<OrderAddress> OrdersAddresses { get; set; } = null!;
    public DbSet<Email> Emails { get; set; } = null!;
    public DbSet<EmailTemplate> EmailTemplates { get; set; } = null!;
    public DbSet<RemindPasswordRequest> RemindPasswordRequests { get; set; } = null!;
    public DbSet<TaxRate> TaxRates { get; set; } = null!;
    public DbSet<UserPreferences> UserPreferences { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimedEntities();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimedEntities()
    {
        var addedEntries = ChangeTracker.Entries<ITimeCreated>()
            .Where(x => x.State == EntityState.Added);
        foreach (var entry in addedEntries)
        {
            entry.Property(x => x.CreatedDate).CurrentValue = _clock.UtcNow.UtcDateTime;
        }

        var modifiedEntries = ChangeTracker.Entries<ITimeModified>()
            .Where(x => x.State == EntityState.Modified);
        foreach (var entry in modifiedEntries)
        {
            entry.Property(x => x.ModifiedDate).CurrentValue = _clock.UtcNow.UtcDateTime;
        }
    }
}