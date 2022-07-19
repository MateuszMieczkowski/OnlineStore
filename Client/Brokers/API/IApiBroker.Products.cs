using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Shared.Models;

namespace SneakersBase.Client.Brokers.API
{
    public partial interface IApiBroker
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<bool> PostProductsAsync(List<CreateProductDto> dtos);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<bool> RemoveProductAsync(int id);

    }
}
