using AuthenticationService.Shared.Dtos.ApplicationUser;

namespace AuthenticationService.Shared.Interfaces.Services
{
    /// <summary>
    /// Defines a contract for sending various types of emails to a specific user, such as welcome emails, confirmation emails, and others.
    /// Implement this interface to provide email-sending functionality for different use cases.
    /// This interface is covariant, allowing instances of <see cref="IUserEmailService{TDto}"/> 
    /// to be assigned to <see cref="IUserEmailService{object}"/>.
    /// </summary>
    public interface IUserEmailService<out TDto>
    {
        /// <summary>
        /// Sends an email directly through this service. This method should handle the logic for constructing
        /// and delivering the email, such as using an SMTP client or an internal email-sending mechanism.
        /// </summary>
        /// <param name="applicationUserId">The Id of the user to whom the email will be sent.</param>
        void SendEmail(object applicationUserId);

        /// <summary>
        /// Sends an email in the form of a notification through an external API. This method should handle
        /// the logic for communicating with the external notification service, including preparing the payload
        /// and making the API request.
        /// </summary>
        /// <param name="applicationUserId">The Id of the user to whom the notification email will be sent.</param>
        void SendNotification(object applicationUserId);
    }
}
