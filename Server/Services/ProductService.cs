using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SneakerBase.Shared.Dtos;

namespace SneakersBase.Server.Services
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAll();
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
    }
}
