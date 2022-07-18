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
        IEnumerable<ProductDto> GetAll();
        IEnumerable<ProductDto> GetBySerach(string filter);
        void CreateMany(IEnumerable<CreateProductDto> dtos);
        void RemoveById(int id);
        ProductDto Update(int id, UpdateProductDto product);
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

        public IEnumerable<ProductDto> GetAll()
        {
            var products = _dbContext.Products
                .Include(p => p.AvailableSizes)
                .ThenInclude(s => s.Size)
                .ToList();
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return productsDto;
        }

        public IEnumerable<ProductDto> GetBySerach(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return GetAll();
            }
            var products = _dbContext.Products
                .Include(p => p.AvailableSizes)
                .ThenInclude(s => s.Size)
                .Where(p => p.Name.Contains(filter) || p.ReferenceNumber.Contains(filter))
                .ToList();
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

        public void CreateMany(IEnumerable<CreateProductDto> dtos)
        {
            var products = _mapper.Map<List<Product>>(dtos);
            _dbContext.Products.AddRange(products);
            _dbContext.SaveChanges();
        }

        public void RemoveById(int id)
        {
            var product = GetById(id);

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }
        public ProductDto Update(int id, UpdateProductDto dto)
        {
            var product = GetById(id, true);

            product.Name = dto.Name;
            product.ReferenceNumber = dto.ReferenceNumber;
            product.ThumbnailPath = dto.ThumbnailPath;
            product.AvailableSizes = dto.AvailableSizes.Select(s => new ProductSize()
            {
                Quantity = s.Quantity,
                SizeId = s.SizeId,
                ProductId = product.Id
            }).ToList();

            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            var productDto = _mapper.Map<ProductDto>(GetById(id, true));
            return productDto;
        }

       
    }
    
}
