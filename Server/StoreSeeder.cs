using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;

namespace OnlineStore.Server;

public class StoreSeeder
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public StoreSeeder(OnlineStoreDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void Seed()
    {
        if (!_dbContext.Database.CanConnect()) return;

        var pendingMigrations = _dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any()) _dbContext.Database.Migrate();

        if (!_dbContext.TaxRates.Any())
        {
            var taxRates = GetTaxRates();
            _dbContext.TaxRates.AddRange(taxRates);
            _dbContext.SaveChanges();
        }

        if (!_dbContext.Roles.Any())
        {
            var roles = GetRoles();
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();
        }

        if (!_dbContext.Users.Any(x => x.Role.Name == UserRoles.Admin))
        {
            var roleId = _dbContext.Roles.First(x => x.Name == UserRoles.Admin).Id;
            var admin = new User()
            {
                Email = "admin@onlinestore.pl",
                RoleId = roleId
            };
            var password = _passwordHasher.HashPassword(admin, "admin123");
            admin.PasswordHash = password;
            _dbContext.Add(admin);
            _dbContext.SaveChanges();
        }
    }

    private IEnumerable<TaxRate> GetTaxRates()
    {
        return new[]
        {
            new TaxRate()
            {
                Amount = 23,
                Description = "23%"
            },
            new TaxRate()
            {
                Amount = 8,
                Description = "8%"
            },
            new TaxRate()
            {
                Amount = 5,
                Description = "5%",
            },
            new TaxRate() {
                Amount = 0,
                Description = "0%"
            },
            new TaxRate() {
                Amount = 0,
                Description = "ZW"
            },
            new TaxRate() {
                Amount = 0,
                Description = "NP"
            }
        };
    }


    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>
        {
            new()
            {
                Name = UserRoles.Admin
            },
            new()
            {
                Name = UserRoles.User
            }
        };

        return roles;
    }
}