using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models.Entities;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthenticationServiceDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return (await GetAsync(t => t.Token == refreshToken)).FirstOrDefault();
        }

        public async Task<RefreshToken?> GetNonRevokedByUserIdAsync(string userId)
        {
            return (await GetAsync(t => t.UserId == userId && t.IsRevoked == false)).FirstOrDefault();
        }

        public async Task<List<RefreshToken>> GetAllByUserIdAsync(string userId)
        {
            return (await GetAsync(t => t.UserId == userId)).ToList();
        }
    }
}
