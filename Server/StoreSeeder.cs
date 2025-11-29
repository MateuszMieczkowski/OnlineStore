using Microsoft.AspNetCore.Identity;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Services.Email;

namespace OnlineStore.Server;

public class StoreSeeder
{
    private readonly EmailTemplateSeeder _emailTemplateSeeder;
    private readonly OnlineStoreDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public StoreSeeder(EmailTemplateSeeder emailTemplateSeeder, OnlineStoreDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _emailTemplateSeeder = emailTemplateSeeder;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!await _dbContext.Database.CanConnectAsync()) return;
        
        // var pendingMigrations = _dbContext.Database.GetPendingMigrations();
        // if (pendingMigrations.Any()) _dbContext.Database.Migrate();

        if (!_dbContext.EmailTemplates.Any())
        {
            await _emailTemplateSeeder.SeedAsync();
        }

        if (!_dbContext.TaxRates.Any())
        {
            var taxRates = GetTaxRates();
            _dbContext.TaxRates.AddRange(taxRates);
            await _dbContext.SaveChangesAsync();
        }

        if (!_dbContext.Users.Any(x => x.UserRole == UserRole.Admin))
        {
            var admin = new User
            {
                Email = "admin@onlinestore.pl",
                UserRole = UserRole.Admin
            };
            var password = _passwordHasher.HashPassword(admin, "admin123");
            admin.PasswordHash = password;
            _dbContext.Add(admin);
            await _dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<TaxRate> GetTaxRates()
        =>
        [
            new() { Amount = 23, Description = "23%" },
            new() { Amount = 8, Description = "8%" },
            new() { Amount = 5, Description = "5%" }, 
            new() { Amount = 0, Description = "0%" },
            new() { Amount = 0, Description = "ZW" },
            new() { Amount = 0, Description = "NP" }
        ];
}