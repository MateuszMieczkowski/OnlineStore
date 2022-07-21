using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Client.Providers;
using SneakersBase.Client.Shared;

namespace SneakersBase.Client.Brokers.API
{
    public partial class ApiBroker : IApiBroker
    {
        private readonly HttpClient _httpClient;
        private readonly ApiAuthenticationStateProvider _authenticationStateProvider;

        public ApiBroker(HttpClient httpClient, ApiAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<T> GetAsync<T>(string relativeUrl) => await _httpClient.GetFromJsonAsync<T>(relativeUrl);

        public async Task<T> GetWithAuthAsync<T>(string relativeUrl)
        {
            var token = await _authenticationStateProvider.GetAuthenticationJwtToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<T>(relativeUrl);
        }



        public async Task<bool> PostAsync<T>(string relativeUrl, T content)
        {
            using var response = await _httpClient.PostAsJsonAsync<T>(relativeUrl, content);
            return await Validate(response);
        }
     
        public async Task<TDto> PostAsync<TPostDto, TDto>(string relativeUrl, TPostDto content)
        {
            using var response = await _httpClient.PostAsJsonAsync(relativeUrl, content);
            await Validate(response);

            return await response.Content.ReadFromJsonAsync<TDto>();
        }

        public async Task<bool> PutAsync<T>(string relativeUrl, T content)
        {
            using var response = await _httpClient.PutAsJsonAsync(relativeUrl, content);
            return await Validate(response);
        }

        public async Task<TDto> PutAsync<TPutDto, TDto>(string relativeUrl, TPutDto content)
        {
            using var response = await _httpClient.PutAsJsonAsync(relativeUrl, content);
            await Validate(response);

            return await response.Content.ReadFromJsonAsync<TDto>();
        }

        public async Task<bool> DeleteAsync(string relativeUrl)
        {
            using var response = await _httpClient.DeleteAsync(relativeUrl);

            return await Validate(response);
        }

    
        private async Task<bool> Validate(HttpResponseMessage? response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string exceptionMessage = await response.Content.ReadAsStringAsync();

                throw new Exception("Something went wrong: " + exceptionMessage);
            }

            return true;
        }
    }
}
