using AuthenticationService.Shared.Dtos.Authentication;

namespace AuthenticationService.Shared.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenDto?> GetByAsync(string refreshToken);
        Task<RefreshTokenDto?> GetNonRevokedByAsync(string userId);
        Task CreateAsync(string userId, string token, DateTime expiresAt);
        Task RevokeAsync(string refreshToken);
        Task RevokeAllForAsync(string userId);
    }
}
