﻿using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Products.Models;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Services;

public interface IProductService
{
    Task<PagedResult<ProductListItemDto>> GetProductList(int pageNumber = 1, int pageSize = 50);
    
    Task<ProductDto> GetProductById(int id);
    
    Task CreateProducts(ICollection<CreateProductModel> products);
    Task Update(int id, UpdateProductModel model);
    Task<bool> Remove(int id);
}

public class ProductService : IProductService
{
    private readonly IApiBroker _broker;

    public ProductService(IApiBroker broker)
    {
        _broker = broker;
    }

    public async Task<PagedResult<ProductListItemDto>> GetProductList(int pageNumber = 1, int pageSize = 50)
    {
        var query = new GetProductList(pageNumber, pageSize, IncludeDeleted: false, IncludeHidden: false);
        return await _broker.GetProductsAsync(query);
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var query = new GetProduct(Id: id, IncludeDeleted: true, IncludeHidden: true);
        return await _broker.GetProductByIdAsync(query);
    }

    public async Task CreateProducts(ICollection<CreateProductModel> products)
    {
        var commandProducts = products.Select(
                x =>
                {
                    var files = x.ProductFiles
                        .Select(y => new CreateProductFile(y.FileName, y.FileBase64 ?? "", y.ProductFileType, y.Description))
                        .ToList();
                    return new CreateProductDto(
                        Name: x.Name,
                        ReferenceNumber: x.ReferenceNumber,
                        ShortDescription: x.ShortDescription,
                        Description: x.Description,
                        Quantity: x.Quantity,
                        PriceNet: x.PriceNet,
                        IsHidden: x.IsHidden,
                        TaxRateId: (int)x.TaxRate,
                        ProductFiles: files);
                })
            .ToList();

        var command = new CreateProductsBatch(commandProducts);
        
        await _broker.PostProductsAsync(command);
    }

    public async Task Update(int id, UpdateProductModel model)
    {
        var dtoFiles = model.ProductFiles.Select(x => new UpdateProductFileDto(x.Id, x.FileName, x.FileBase64, x.ProductFileType, x.Description)).ToList();
        var dto = new UpdateProductDto(
            Name: model.Name,
            ReferenceNumber: model.ReferenceNumber,
            ShortDescription: model.ShortDescription,
            Description: model.Description,
            Quantity: model.Quantity,
            PriceNet: model.PriceNet,
            IsHidden: model.IsHidden,
            IsDeleted: false,
            TaxRateId: (int)model.TaxRate,
            ProductFiles: dtoFiles);
        
        await _broker.UpdateProductAsync(id, dto);
    }

    public async Task<bool> Remove(int id)
    {
        return await _broker.RemoveProductAsync(id);
    }
}