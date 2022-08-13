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
        Task<List<SizeDto>> GetSizesAsync();
        Task<SizeDto> PostSizeAsync(CreateSizeDto dto);
        Task<SizeDto> UpdateSizeAsync(Guid id, UpdateSizeDto dto);
        Task<bool> RemoveSizeAsync(Guid id);
    }
}
