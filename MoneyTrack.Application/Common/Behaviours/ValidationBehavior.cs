using FluentValidation;
using MediatR;

namespace MoneyTrack.Application.Common.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Select(f => f.ErrorMessage)
                .Where(f => f != null)
                .ToList();
            
            if (failures.Count > 0)
            {
                throw new MoneyTrack.Application.Exceptions.ValidationException() { ValidationErrors = failures };
            }
        }

        return await next();
    }
}