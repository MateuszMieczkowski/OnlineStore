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
        Task<bool> Remove(int id);
        Task<SizeDto> Update(int id, UpdateSizeDto dto);
    }

    public class SizeService : ISizeService
    {
        private readonly HttpClient _http;
        private readonly IApiBroker _apiBroker;

        public SizeService(HttpClient http, IApiBroker apiBroker)
        {
            _http = http;
            _apiBroker = apiBroker;
        }
        public async Task<List<SizeDto>> GetAllAsync()
        {
            var sizeDtos = await _apiBroker.GetAllSizesAsync();
         //   var sizeDtos = await _http.GetFromJsonAsync<List<SizeDto>>("/api/size");
            return sizeDtos;
        }

        public async Task<SizeDto> Create(CreateSizeDto dto)
        {
            using var response = await _http.PostAsJsonAsync("api/size", dto);

            if (!response.IsSuccessStatusCode)
            {
                string exceptionMessage = await response.Content.ReadAsStringAsync();

                throw new Exception("Something went wrong: " + exceptionMessage);
            }
            var sizeDto = await response.Content.ReadFromJsonAsync<SizeDto>();
            return sizeDto;
        }

        public async Task<bool> Remove(int id)
        {
            using var response = await _http.DeleteAsync($"api/size/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<SizeDto> Update(int id, UpdateSizeDto dto)
        {
            using var response = await _http.PutAsJsonAsync($"api/size/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong: " + response.ReasonPhrase);
            }
            var sizeDto = await response.Content.ReadFromJsonAsync<SizeDto>();
            return sizeDto;
        }
    }
}
