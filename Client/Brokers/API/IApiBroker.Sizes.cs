using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<List<SizeDto>> GetSizesAsync();
    Task<SizeDto> PostSizeAsync(CreateSizeDto dto);
    Task<SizeDto> UpdateSizeAsync(Guid id, UpdateSizeDto dto);
    Task<bool> RemoveSizeAsync(Guid id);
}