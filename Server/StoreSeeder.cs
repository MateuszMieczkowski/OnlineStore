using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;

namespace OnlineStore.Server;

public class StoreSeeder(OnlineStoreDbContext dbContext, IPasswordHasher<User> passwordHasher)
{
    public void Seed()
    {
        if (!dbContext.Database.CanConnect()) return;

        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any()) dbContext.Database.Migrate();

        SeedTaxRates();
        SeedUsers();
        SeedProducts();
    }
    private void SeedUsers()
    {
        if (!dbContext.Users.Any(x => x.UserRole == UserRole.Admin))
        {
            var admin = new User()
            {
                Email = "admin@onlinestore.pl",
                UserRole = UserRole.Admin
            };
            var password = passwordHasher.HashPassword(admin, "admin123");
            admin.PasswordHash = password;
            dbContext.Add(admin);
            dbContext.SaveChanges();
        }
    }
    private void SeedTaxRates()
    {
        if (!dbContext.TaxRates.Any())
        {
            var taxRates = GetTaxRates();
            dbContext.TaxRates.AddRange(taxRates);
            dbContext.SaveChanges();
        }
    }

    private void SeedProducts()
    {
        if (!dbContext.Products.Any())
        {
            var products = GetProducts();
            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
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
    
    private IEnumerable<Product> GetProducts()
    {
        return
        [
            new Product
            {
                Name = "Sample Product 1",
                ShortDescription = "Sample Product Short Description 1",
                Description = "Sample Product Description 1",
                ReferenceNumber = "SP001",
                Quantity = 150,
                PriceGross = 100,
                PriceNet = 77,
                ProductFiles = [],
                TaxRate = new TaxRate { Amount = 23, Description = "23%" },
                TaxRateId = 1 // 23%
            },
            new Product
            {
                Name = "Sample Product 2",
                ShortDescription = "Sample Product Short Description 2",
                Description = "Sample Product Description 2",
                ReferenceNumber = "SP002",
                Quantity = 250,
                PriceGross = 200,
                PriceNet = 184,
                ProductFiles = [],
                TaxRate = new TaxRate { Amount = 8, Description = "8%" },
                TaxRateId = 2 // 8%
            },
            new Product
            {
                Name = "Sample Product 3",
                ShortDescription = "Sample Product Short Description 3",
                Description = "Sample Product 3",
                ReferenceNumber = "SP003",
                Quantity = 350,
                PriceGross = 300,
                PriceNet = 285,
                TaxRate = new TaxRate { Amount = 5, Description = "5%" },
                ProductFiles = [] // 5%
            }
        ];
    }
}