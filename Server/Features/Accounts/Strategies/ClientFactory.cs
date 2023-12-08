using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Shared.Clients;

namespace OnlineStore.Server.Features.Accounts.Strategies;

public class ClientFactory : IUserFactory<RegisterClient>
{
    public User Create(RegisterClient command)
    {
        return new Client
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            UserRole = UserRole.User
        };
    }
}
