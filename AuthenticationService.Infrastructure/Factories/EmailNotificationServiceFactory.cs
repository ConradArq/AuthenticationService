using AuthenticationService.Shared.Interfaces.Factories;
using AuthenticationService.Shared.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Factories
{
    public class EmailNotificationServiceFactory : IEmailNotificationServiceFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EmailNotificationServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IUserEmailService<TDto> Create<TDto>()
        {
            // Singletons like this factory are resolved from the root service provider, which cannot resolve scoped services directly.
            // Scoped services require a child service provider (created within a scope) for proper lifetime management.
            // Do not inject IServiceScope directly — doing so in a singleton would tie the scope to the lifetime of the singleton,
            // which lives for the entire application lifetime, preventing proper disposal of scoped services and causing memory leaks.
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                var serviceType = typeof(IUserEmailService<TDto>);
                return (IUserEmailService<TDto>)scopedProvider.GetRequiredService(serviceType);
            }
        }
    }
}
