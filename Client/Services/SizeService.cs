using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Client.Brokers.API;
using SneakersBase.Shared.Models;


namespace SneakersBase.Client.Services
{
    public interface ISizeService
    {
        Task<List<SizeDto>> GetAllAsync();
        Task<SizeDto> Create(CreateSizeDto dto);
        Task<bool> Remove(Guid id);
        Task<SizeDto> Update(Guid id, UpdateSizeDto dto);
    }

    public class SizeService : ISizeService
    {
        private readonly IApiBroker _apiBroker;

        public SizeService(IApiBroker apiBroker)
        {
            _apiBroker = apiBroker;
        }
        public async Task<List<SizeDto>> GetAllAsync() => await _apiBroker.GetSizesAsync();

        public async Task<SizeDto> Create(CreateSizeDto dto)
        {
            return await _apiBroker.PostSizeAsync(dto);
        }

        public async Task<bool> Remove(Guid id)
        {
            return await _apiBroker.RemoveSizeAsync(id);
        }

        public async Task<SizeDto> Update(Guid id, UpdateSizeDto dto)
        {
            return await _apiBroker.UpdateSizeAsync(id, dto);
        }
    }
}
