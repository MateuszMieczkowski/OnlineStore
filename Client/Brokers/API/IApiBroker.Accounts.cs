using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Shared.Models;

namespace SneakersBase.Client.Brokers.API
{
    public partial interface IApiBroker
    {
        Task<AuthResponse> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(RegisterUserDto registerDto);
    }
}
