using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string SizeRelativeUrl = "api/size";

    public async Task<List<SizeDto>> GetSizesAsync()
    {
        return await GetWithAuthAsync<List<SizeDto>>(SizeRelativeUrl);
    }

    public async Task<SizeDto> PostSizeAsync(CreateSizeDto dto)
    {
        return await PostAsync<CreateSizeDto, SizeDto>(SizeRelativeUrl, dto);
    }

    public async Task<SizeDto> UpdateSizeAsync(Guid id, UpdateSizeDto dto)
    {
        return await PutAsync<UpdateSizeDto, SizeDto>(SizeRelativeUrl + $"/{id}", dto);
    }

    public async Task<bool> RemoveSizeAsync(Guid id)
    {
        return await DeleteAsync(SizeRelativeUrl + $"/{id}");
    }
}