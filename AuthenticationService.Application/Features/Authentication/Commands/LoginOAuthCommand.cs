
namespace AuthenticationService.Application.Features.Authentication.Commands
{
    // Common command model for all OAuth provider login requests.
    public class LoginOAuthCommand
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
