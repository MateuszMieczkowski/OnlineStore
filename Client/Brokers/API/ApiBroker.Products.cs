﻿using System.Text;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ProductRelativeUrl = "api/products";

    public async Task<PagedResult<ProductListItemDto>> GetProductsAsync(GetProductList query)
    {
        var queryParams = new StringBuilder()
            .Append($"?pageNumber={query.PageNumber}")
            .Append($"&pageSize={query.PageSize}")
            .Append($"&searchPhrase={query.SearchPhrase}")
            .Append($"&name={query.Name}")
            .Append($"&referenceNumber={query.ReferenceNumber}")
            .Append($"&shortDescription={query.ShortDescription}")
            .Append($"&filterGrossPrice={query.FilterGrossPrice}")
            .Append($"&priceFrom={query.PriceFrom}")
            .Append($"&priceTo={query.PriceTo}")
            .Append($"&hiddenOnly={query.HiddenOnly}")
            .Append($"&deletedOnly={query.DeletedOnly}")
            .ToString();
        
        return await GetAsync<PagedResult<ProductListItemDto>>($"{ProductRelativeUrl}{queryParams}");
    }

    public async Task<ProductDto> GetProductByIdAsync(GetProduct query)
    {
        return await GetAsync<ProductDto>($"{ProductRelativeUrl}/{query.Id}");
    }

    public async Task PostProductsAsync(CreateProductsBatch command)
    {
        await PostAsync($"{ProductRelativeUrl}/create-batch", command);
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto dto)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}", dto);
    }


    public async Task HideProductAsync(int id)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}/hide");
    }
    
    public async Task RecoverProductAsync(int id)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}/recover");
    }
    
    public async Task RevealProductAsync(int id)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}/reveal");
    }

    public async Task SoftDeleteProductAsync(int id)
    {
        await DeleteAsync($"{ProductRelativeUrl}/{id}/soft-delete");
    }
    
    public async Task HardDeleteProductAsync(int id)
    {
        await DeleteAsync($"{ProductRelativeUrl}/{id}/hard-delete");
    }

    public async Task EmptyProductsBinAsync(IEnumerable<int> ids)
    {
        var deleteTasks = ids.Select(HardDeleteProductAsync);
        await Task.WhenAll(deleteTasks);
    }

    public async Task<IReadOnlyCollection<TaxRateDto>> GetTaxRates()
    {
        return await GetAsync<IReadOnlyCollection<TaxRateDto>>($"{ProductRelativeUrl}/taxRates");
    }
}