using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SneakerBase.Shared.Dtos;

namespace SneakersBase.Client.Services
{
    public interface ISizeService
    {
        Task<List<SizeDto>> GetAllAsync();

    }
    public class SizeService : ISizeService
    {
        private readonly HttpClient _http;
        public SizeService(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<SizeDto>> GetAllAsync()
        {
            var sizeDtos = await _http.GetFromJsonAsync<List<SizeDto>>("/api/size");
            return sizeDtos;
        }
    }
}
