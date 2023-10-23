using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<AuthResponse> LoginAsync(LoginDto loginDto);
    Task RegisterAsync(RegisterUserDto registerDto);
}