using OnlineStore.Server.Accounts.Strategies;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Shared.Clients;

namespace OnlineStore.Server.Clients.Factories;

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
            IsSubscribedToNewsletter = command.IsSubscribedToNewsletter,
            UserRole = UserRole.User
        };
    }
}
