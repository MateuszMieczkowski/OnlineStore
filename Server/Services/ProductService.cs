using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Services;

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
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;

    public ProductService(OnlineStoreDbContext dbContext, IMapper mapper, ILogger<ProductService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .Include(p => p.AvailableSizes)
            .ThenInclude(s => s.Size)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        var productsDto = _mapper.Map<List<ProductDto>>(products);
        return productsDto;
    }

    public async Task<IEnumerable<ProductDto>> GetAllTest()
    {
        var products = await _dbContext.Products
            .Include(p => p.AvailableSizes)
            .ThenInclude(s => s.Size)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        var productsDto = _mapper.Map<List<ProductDto>>(products);
        return productsDto;
    }


    public async Task<IEnumerable<ProductDto>> GetBySerachAsync(string filter)
    {
        if (string.IsNullOrEmpty(filter)) return await GetAllAsync();
        var products = _dbContext.Products
            .AsNoTracking()
            .Include(p => p.AvailableSizes)
            .ThenInclude(s => s.Size)
            .Where(p => p.Name.Contains(filter) || p.ReferenceNumber.Contains(filter))
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        var productsDto = _mapper.Map<List<ProductDto>>(products);
        return productsDto;
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
        var product = await _dbContext.Products
            .Include(p => p.AvailableSizes)
            .ThenInclude(s => s.Size)
            .FirstAsync(p => p.Id == id);

        product.Name = dto.Name;
        product.ReferenceNumber = dto.ReferenceNumber;
        product.AvailableSizes = dto.AvailableSizes.Select(s => new ProductSize
        {
            Quantity = s.Quantity,
            SizeId = s.SizeId,
            Size = !s.SizeId.HasValue && !string.IsNullOrEmpty(s.Size) ? new Size { Name = s.Size } : null,
            ProductId = product.Id
        }).ToList();

        await _dbContext.SaveChangesAsync();

        var productDto = _mapper.Map<ProductDto>(GetById(id, true));
        return productDto;
    }

    public Product GetById(int id, bool hasIncluds = false)
    {
        Product? product;
        if (hasIncluds)
            product = _dbContext
                .Products
                .Include(p => p.AvailableSizes)
                .ThenInclude(s => s.Size)
                .FirstOrDefault(p => p.Id == id);
        else
            product = _dbContext
                .Products
                .FirstOrDefault(p => p.Id == id);


        if (product is null)
            throw new NotFoundException("Product not found");
        return product;
    }
}