using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Shared.Models;

namespace SneakersBase.Client.Brokers.API
{
    public partial class ApiBroker
    {
        private const string ProductRelativeUrl = "api/products";
        public async Task<List<ProductDto>> GetProductsAsync() =>
            await GetAsync<List<ProductDto>>(ProductRelativeUrl);

        public async Task<bool> PostProductsAsync(List<CreateProductDto> dtos) =>
            await PostAsync(ProductRelativeUrl, dtos);

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto) =>
            await PutAsync<UpdateProductDto, ProductDto>(ProductRelativeUrl + $"/{id}", dto);

        public async Task<bool> RemoveProductAsync(int id) =>
            await DeleteAsync(ProductRelativeUrl + $"/{id}");
    }
}
