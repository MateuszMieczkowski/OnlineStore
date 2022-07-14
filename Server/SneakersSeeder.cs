﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SneakerBase.Entities;

namespace SneakersBase.Server
{
    public class SneakersSeeder
    {
        private readonly SneakersDbContext _dbContext;

        public SneakersSeeder(SneakersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Products.Any())
                {
                    var products = GetProducts();
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }
            }

        }

        public static IEnumerable<Product> GetProducts()
        {
            var list = Enumerable.Range(1, 20).Select(index => new Product()
            {
                Name = $"Jordan 1 Mid Triple White 2.0 ({Random.Shared.Next(2012, 2022)})",
                ReferenceNumber = $"{Random.Shared.Next(111111, 9999999)}-{Random.Shared.Next(100, 999)}",
                ThumbnailPath = $"sneakers/sneaker{Random.Shared.Next(2, 6)}.png",
                AvailableSizes = new List<ProductSize>()
                {
                    new ProductSize()
                    {
                        Size = new Size()
                        {
                            Name = "43"
                        },
                        Quantity = Random.Shared.Next(10, 55)
                    },
                    new ProductSize()
                    {
                        Size = new Size()
                        {
                            Name = "44"
                        },
                        Quantity = Random.Shared.Next(10, 55)
                    },
                    new ProductSize()
                    {
                        Size = new Size()
                        {
                            Name = "45"
                        },
                        Quantity = Random.Shared.Next(10, 55)
                    },
                    new ProductSize()
                    {
                        Size = new Size()
                        {
                            Name = "42"
                        },
                        Quantity = Random.Shared.Next(10, 55)
                    }
                },
            }).ToList();

            return list;
        }
    }
}
