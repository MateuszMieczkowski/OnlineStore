using MediatR;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Server.Infrastructure;

public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand> 
    where TCommand : ICommand
{
}


