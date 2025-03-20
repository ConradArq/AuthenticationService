using AuthenticationService.Shared.Interfaces.Services;

namespace AuthenticationService.Shared.Interfaces.Factories
{
    public interface IEmailNotificationServiceFactory
    {
        IUserEmailService<TDto> Create<TDto>();
    }
}