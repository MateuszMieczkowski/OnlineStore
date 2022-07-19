using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SneakersBase.Shared.Models;

namespace SneakersBase.Client.Brokers.API
{
    public partial class ApiBroker
    {
        private const string AccountRelativeUrl = "api/account";

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(AccountRelativeUrl + "/login", loginDto);

            var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();

          return auth;
        }

        public async Task RegisterAsync(RegisterUserDto registerDto)
        {
            await PostAsync(AccountRelativeUrl + "/register", registerDto);
        }
    }
}
