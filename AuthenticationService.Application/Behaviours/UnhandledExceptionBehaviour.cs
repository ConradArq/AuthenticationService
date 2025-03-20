using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

        public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred in MediatR pipeline behavior for request {RequestType}. Request: {@Request}", typeof(TRequest).Name, request);
                throw; /// Exception will be handled by <see cref="ExceptionMiddleware"/>, which is responsible for handling exceptions within the HTTP pipeline and creating appropriate HTTP responses (e.g., setting status codes and formatting JSON responses).
            }
        }
    }
}
