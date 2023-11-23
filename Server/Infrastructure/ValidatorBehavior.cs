using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace OnlineStore.Server.Infrastructure;

public class ValidatorBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this._validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        
        var context = new ValidationContext<TRequest>(request);
        var errors = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null);

        var validationFailures = errors as ValidationFailure[] 
            ?? errors.ToArray();
        
        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }
        return await next();
    }
}