using AuthenticationService.Infrastructure.Persistence;
using System.Security.Claims;

namespace AuthenticationService.API.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Middleware responsible for setting the current user's ID in the DbContext.
        /// This ID is automatically assigned to the CreatedBy and LastModifiedBy properties of each entity during data operations.
        /// </summary>
        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthenticationServiceDbContext dbContext)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
            dbContext.CurrentUserId = userId;

            await _next(context);
        }
    }
}
