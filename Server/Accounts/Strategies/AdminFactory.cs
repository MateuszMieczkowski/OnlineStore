using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Strategies;

public class AdminFactory : IUserFactory<RegisterAdmin>
{
    public User Create(RegisterAdmin command)
    {
        return new User
        {
            Email = command.Email,
            UserRole = UserRole.Admin
        };
    }
}
