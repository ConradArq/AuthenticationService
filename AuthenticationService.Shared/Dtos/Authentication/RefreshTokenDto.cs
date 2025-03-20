
namespace AuthenticationService.Shared.Dtos.Authentication
{
    public class RefreshTokenDto
    {
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReplacedByToken { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
