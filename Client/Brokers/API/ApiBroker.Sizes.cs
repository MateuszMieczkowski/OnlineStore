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
        private const string SizeRelativeUrl = "api/size";

        public async Task<List<SizeDto>> GetSizesAsync() => await GetWithAuthAsync<List<SizeDto>>(SizeRelativeUrl);

        public async Task<SizeDto> PostSizeAsync(CreateSizeDto dto) =>
            await PostAsync<CreateSizeDto, SizeDto>(SizeRelativeUrl, dto);

        public async Task<SizeDto> UpdateSizeAsync(int id, UpdateSizeDto dto) =>
            await PutAsync<UpdateSizeDto, SizeDto>(SizeRelativeUrl + $"/{id}", dto);

        public async Task<bool> RemoveSizeAsync(int id) =>
            await DeleteAsync(SizeRelativeUrl + $"/{id}");
    }
}
