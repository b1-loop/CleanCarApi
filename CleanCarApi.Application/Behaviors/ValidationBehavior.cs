using FluentValidation;
using MediatR;

namespace CleanCarApi.Application.Behaviors
{
    // Pipeline Behaviour körs automatiskt innan varje handler
    // Här validerar vi inkommande requests med FluentValidation
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        // Alla validators som är registrerade för TRequest injiceras hit
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        // Ny signatur för MediatR v12+: next är en delegate utan CancellationToken
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // Om det finns validators, kör dem alla
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // Kör alla validators parallellt och samla resultaten
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // Samla ihop alla fel från alla validators
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // Om det finns fel, kasta ett undantag innan handler körs
                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }

            // Inga fel — gå vidare till nästa steg i pipeline (handlern)
            return await next();
        }
    }
}
