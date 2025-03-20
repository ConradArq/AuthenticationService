using AuthenticationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
        Task<List<RefreshToken>> GetAllByUserIdAsync(string userId);
        Task<RefreshToken?> GetNonRevokedByUserIdAsync(string userId);
    }
}
