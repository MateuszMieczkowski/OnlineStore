using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Client.Brokers.API;
using SneakersBase.Shared.Models;

namespace SneakersBase.Client.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<bool> CreateProducts(List<CreateProductDto> dtos);
        Task<ProductDto> Update(int id, UpdateProductDto dto);
        Task<bool> Remove(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IApiBroker _broker;

        public ProductService(IApiBroker broker)
        {
            _broker = broker;
        }

        public async Task<List<ProductDto>> GetAllAsync() =>
            await _broker.GetProductsAsync();

        public async Task<bool> CreateProducts(List<CreateProductDto> dtos) =>
            await _broker.PostProductsAsync(dtos);

        public async Task<ProductDto> Update(int id, UpdateProductDto dto) =>
            await _broker.UpdateProductAsync(id, dto);

        public async Task<bool> Remove(int id) =>
            await _broker.RemoveProductAsync(id);
    }
}
