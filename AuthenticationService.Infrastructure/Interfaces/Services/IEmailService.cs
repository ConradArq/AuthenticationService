using AuthenticationService.Domain.Models;

namespace AuthenticationService.Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Defines a contract for sending emails. Implement this interface with low-level logic
    /// to handle the actual process of sending an email, such as connecting to an SMTP server
    /// or using a third-party email service.
    /// </summary>
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
    }
}
