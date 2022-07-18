﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Client.Brokers.API
{
    public partial class ApiBroker : IApiBroker
    {
        private readonly HttpClient _httpClient;

        public ApiBroker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string relativeUrl) => await _httpClient.GetFromJsonAsync<T>(relativeUrl);


        public async Task PostAsync<T>(string relativeUrl, T content) =>
            await _httpClient.PostAsJsonAsync<T>(relativeUrl, content);


    }
}
