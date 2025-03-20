using AuthenticationService.Shared.Dtos.ApplicationUser;
using AuthenticationService.Shared.Dtos.Authentication;
using System.Security.Claims;

namespace AuthenticationService.Shared.Interfaces.Providers
{
    public interface IJwtTokenProvider
    {
        Task<string> GenerateAuthenticationTokenAsync(object applicationUserId, TimeSpan? expirationTime = null, params Claim[] additionalClaims);
        Task<string> GenerateAuthenticationTokenAsync(string oldToken, TimeSpan? expirationTime = null, params Claim[] additionalClaims);
        Task<string> GenerateAuthenticationTokenAsync(TimeSpan? expirationTime = null, params Claim[] claims);
        string? GetUserAuthenticationToken();
        ApplicationUserResponseDto GetUserDataFromFromAuthenticationToken();
        JwtSettingsDto GetJwtSettings();
    }
}