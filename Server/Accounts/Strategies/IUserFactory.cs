using OnlineStore.Server.Entities;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Strategies;

public interface IUserFactory<in TRegisterCommand>
    where TRegisterCommand : IRegisterUserCommand
{
    User Create(TRegisterCommand command);
}
