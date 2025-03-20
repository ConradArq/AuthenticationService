using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Shared.Resources;

namespace AuthenticationService.API.Middlewares
{
    /// <summary>
    /// Middleware to handle authorization errors by returning custom JSON responses via <see cref="ExceptionMiddleware"/> for 
    /// 401 Unauthorized and 403 Forbidden status codes.
    /// </summary>
    public class AuthErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                throw new UnauthorizedAccessException(GeneralMessages.UnauthorizedAccessExceptionMessage);
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                throw new ForbiddenException();
            }
        }
    }
}
