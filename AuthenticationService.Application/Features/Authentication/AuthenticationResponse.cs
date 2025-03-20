using AuthenticationService.Application.Features.ApplicationUser;

namespace AuthenticationService.Application.Features.Authentication
{
    public class AuthenticationResponse
    {
        public string? AccessToken { get; set; }
        public DateTime? AccessTokenExpirationDate { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationDate { get; set; }

        public ApplicationUserResponse? User { get; set; }
    }
}
