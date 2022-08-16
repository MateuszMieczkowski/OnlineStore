using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SneakersBase.Server.Entities;

namespace SneakersBase.Server
{
    public class SneakersSeeder
    {
        private readonly SneakersDbContext _dbContext;

        public SneakersSeeder(SneakersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedProduction()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                
            }

        }
        public void SeedDebug()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Sizes.Any())
                {
                    var sizes = GetSizes();
                    _dbContext.Sizes.AddRange(sizes);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Products.Any())
                {
                    var products = GetProducts();
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

            }

        }


        public IEnumerable<Product> GetProducts()
        {
            var random = new Random();
            var size = _dbContext.Sizes.First();
            var list = Enumerable.Range(1, 100).Select(index => new Product()
            {
                Name = $"Jordan 1 Mid Triple White 2.0 ({Random.Shared.Next(2012, 2022)})",
                ReferenceNumber = $"{Random.Shared.Next(111111, 9999999)}-{Random.Shared.Next(100, 999)}",
                AvailableSizes = new List<ProductSize>() { new ProductSize(){
                    Quantity = Random.Shared.Next(10, 55),
                    Size = size
                } }
            }).ToList();

            return list;
        }
        private IEnumerable<Size> GetSizes()
        {
            var list = Enumerable.Range(1, 5).Select(index => new Size()
            {
                Name = $"{Random.Shared.Next(35, 46)}",
            }).ToList();

            return list;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }

    }
}
