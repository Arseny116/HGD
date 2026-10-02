using FluentValidation;
using MediatR;

namespace MiroIntegration.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var contexts = validators.Select(x => x.ValidateAsync(request, cancellationToken));
        var results = await Task.WhenAll(contexts);
        var failures = results.SelectMany(x => x.Errors).Where(x => x is not null).ToArray();
        if (failures.Length > 0) throw new ValidationException(failures);
        return await next();
    }
}