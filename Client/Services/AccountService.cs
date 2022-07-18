using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SneakersBase.Client.Brokers.API;
using SneakersBase.Client.Providers;
using SneakersBase.Shared.Models;

namespace SneakersBase.Client.Services
{
    public interface IAccountService
    {
        Task<bool> AuthenticateAsync(LoginDto loginDto);
        Task Logout();
    }

    public class AccountService : IAccountService
    {
        private readonly IApiBroker _broker;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AccountService(IApiBroker broker, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _broker = broker;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(LoginDto loginDto)
        {
            var response = await _broker.LoginAsync(loginDto);

            await _localStorage.SetItemAsync("accessToken", response.Token);

            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
            return true;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }
    }
}
