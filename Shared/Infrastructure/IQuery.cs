using MediatR;

namespace OnlineStore.Shared.Infrastructure;

public interface IQuery<out TDto> : IRequest<TDto>
{
}