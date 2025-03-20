using FluentValidation;
using MediatR;
using ValidationException = AuthenticationService.Shared.Exceptions.ValidationException;

namespace AuthenticationService.Application.Behaviours
{
    /// <summary>
    /// A MediatR pipeline behavior that validates requests before they reach the handler. The MediatR pipeline runs when 
    /// <c>_mediator.Send(request);</c> is called, and behaviors execute first. This behavior captures <see cref="ValidationException"/> 
    /// thrown by handlers (e.g., FluentValidation) before the request reaches the handler.
    /// It uses <see cref="IValidator{T}"/> instances to validate requests implementing <see cref="IRequest{TResponse}"/>.
    /// If validation fails, a <see cref="ValidationException"/> is thrown, preventing invalid requests from being processed.
    /// This centralizes validation logic, removes the need for manual validation, and improves maintainability.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request being validated.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Execute FluentValidation to validate the request
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                var failureDictionary = failures
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(f => f.ErrorMessage).ToArray()
                    );

                throw new ValidationException(failureDictionary);
            }

            return await next();
        }
    }
}
