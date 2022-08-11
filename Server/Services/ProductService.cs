using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SneakersBase.Server.Entities;
using SneakersBase.Server.Services.Exceptions;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<ProductDto>> GetBySerachAsync(string filter);
        Task<List<Product>> CreateManyAsync(IEnumerable<CreateProductDto> dtos);
        void RemoveById(int id);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto product);
    }

    public class ProductService : IProductService
    {
        private readonly SneakersDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(SneakersDbContext dbContext, IMapper mapper, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _dbContext.Products
                .Include(p => p.AvailableSizes)
                .ThenInclude(s => s.Size)
                .ToListAsync();

            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return productsDto;
        }

        public async Task<IEnumerable<ProductDto>> GetBySerachAsync(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return await GetAllAsync();
            }
            var products = _dbContext.Products
                .Include(p => p.AvailableSizes)
                .ThenInclude(s => s.Size)
                .Where(p => p.Name.Contains(filter) || p.ReferenceNumber.Contains(filter))
                .ToListAsync();

            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return productsDto;
        }
        public Product GetById(int id, bool hasIncluds = false)
        {
            Product? product;
            if (hasIncluds)
            {
                product = _dbContext
                    .Products
                    .Include(p => p.AvailableSizes)
                    .ThenInclude(s => s.Size)
                    .FirstOrDefault(p => p.Id == id);
            }
            else
            {
                product = _dbContext
                    .Products
                    .FirstOrDefault(p => p.Id == id);
            }


            if (product is null)
                throw new NotFoundException("Product not found");
            return product;
        }

        public async Task<List<Product>> CreateManyAsync(IEnumerable<CreateProductDto> dtos)
        {
            var products = _mapper.Map<List<Product>>(dtos);

            _dbContext.Products.AddRange(products);
            await _dbContext.SaveChangesAsync();

            return products;
        }

        public void RemoveById(int id)
        {
            var product = GetById(id);

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }
        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            //var product = GetById(id, true);
            var product = await _dbContext.Products
                .Include(p => p.AvailableSizes)
                .ThenInclude(s => s.Size)
                .FirstAsync(p => p.Id == id);

            product.Name = dto.Name;
            product.ReferenceNumber = dto.ReferenceNumber;
            product.AvailableSizes = dto.AvailableSizes.Select(s => new ProductSize()
            {
                Quantity = s.Quantity,
                SizeId = s.SizeId,
                ProductId = product.Id
            }).ToList();

            await _dbContext.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(GetById(id, true));
            return productDto;
        }


    }

}
