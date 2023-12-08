using OnlineStore.Server.Entities;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Features.Accounts.Strategies;

public interface IUserFactory<in TRegisterCommand>
    where TRegisterCommand : IRegisterUserCommand
{
    User Create(TRegisterCommand command);
}
