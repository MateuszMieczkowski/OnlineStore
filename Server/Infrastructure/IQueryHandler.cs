using MediatR;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Server.Infrastructure;

public interface IQueryHandler<in TQuery, TResponse> 
    : IRequestHandler<TQuery, TResponse> 
    where TQuery : IQuery<TResponse>
{
}
