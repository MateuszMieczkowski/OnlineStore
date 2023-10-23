using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Services;

public interface ISizeService
{
    Task<IEnumerable<SizeDto>> GetAll();
    SizeDto Create(CreateSizeDto dto);
    void Remove(Guid id);
    SizeDto Update(Guid id, UpdateSizeDto dto);
    Task<IEnumerable<Size>> GetNewSizesAsync(IEnumerable<CreateProductDto> products);
}

public class SizeService : ISizeService
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<SizeService> _logger;
    private readonly IMapper _mapper;

    public SizeService(OnlineStoreDbContext dbContext, IMapper mapper, ILogger<SizeService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SizeDto>> GetAll()
    {
        var sizes = await _dbContext.Sizes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
        var sizeDtos = _mapper.Map<List<SizeDto>>(sizes);
        return sizeDtos;
    }

    public SizeDto Create(CreateSizeDto dto)
    {
        if (IsSizeDuplicated(dto.Name)) throw new DuplicateException();

        var size = _mapper.Map<Size>(dto);

        _dbContext.Sizes.Add(size);
        _dbContext.SaveChanges();

        var sizeDto = _mapper.Map<SizeDto>(size);
        return sizeDto;
    }

    public void Remove(Guid id)
    {
        var size = GetSizeById(id);
        var productSizes = _dbContext.ProductSizes.Where(x => x.SizeId == id).ToList();

        _dbContext.ProductSizes.RemoveRange(productSizes);
        _dbContext.Sizes.Remove(size);
        _dbContext.SaveChanges();
    }

    public SizeDto Update(Guid id, UpdateSizeDto dto)
    {
        var size = GetSizeById(id);

        IsSizeDuplicated(dto.Name, size.Id);

        size.Name = dto.Name;

        _dbContext.Sizes.Update(size);
        _dbContext.SaveChanges();

        var updatedDto = _mapper.Map<SizeDto>(size);
        return updatedDto;
    }

    public async Task<IEnumerable<Size>> GetNewSizesAsync(IEnumerable<CreateProductDto> products)
    {
        var allSizes = await _dbContext.Sizes.ToListAsync();
        var newSizes = new List<Size>();
        foreach (var product in products)
        {
            var sizesToCreate = product.AvailableSizes.Where(s => !s.SizeId.HasValue && !string.IsNullOrEmpty(s.Size));
            foreach (var productSize in sizesToCreate)
            {
                var existingSize = allSizes.FirstOrDefault(x => x.Name == productSize.Size);
                if (existingSize != null)
                {
                    productSize.SizeId = existingSize.Id;
                    continue;
                }

                productSize.SizeId = Guid.NewGuid();
                var newSize = new Size { Id = productSize.SizeId.Value, Name = productSize.Size };
                allSizes.Add(newSize);
                newSizes.Add(newSize);
            }
        }

        //   _dbContext.Sizes.AddRange(newSizes);
        // await _dbContext.SaveChangesAsync();
        return newSizes;
    }

    private Size GetSizeById(Guid id)
    {
        var size = _dbContext.Sizes.FirstOrDefault(s => s.Id == id);
        if (size is null) throw new NotFoundException("Size not found");
        return size;
    }

    private bool IsSizeDuplicated(string name, Guid exceptionId = default)
    {
        var isDuplicated = _dbContext.Sizes.Any(s => s.Name == name && s.Id != exceptionId);
        if (isDuplicated)
            throw new DuplicateSizeException();
        return isDuplicated;
    }
}