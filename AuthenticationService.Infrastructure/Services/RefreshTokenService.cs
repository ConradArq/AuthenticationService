using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Shared.Interfaces.Services;
using AuthenticationService.Shared.Dtos.Authentication;

namespace AuthenticationService.Infrastructure.Services
{
    internal class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RefreshTokenDto?> GetByAsync(string refreshToken)
        {
            var entity = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);

            if (entity == null)
                return null;

            return new RefreshTokenDto
            {
                Token = entity.Token,
                UserId = entity.UserId,
                IsRevoked = entity.IsRevoked,
                ExpiresAt = entity.ExpiresAt,
                CreatedAt = entity.CreatedAt,
                ReplacedByToken = entity.ReplacedByToken
            };
        }

        public async Task<RefreshTokenDto?> GetNonRevokedByAsync(string userId)
        {
            var entity = await _refreshTokenRepository.GetNonRevokedByUserIdAsync(userId);

            if (entity == null)
                return null;

            return new RefreshTokenDto
            {
                Token = entity.Token,
                UserId = entity.UserId,
                IsRevoked = entity.IsRevoked,
                ExpiresAt = entity.ExpiresAt,
                CreatedAt = entity.CreatedAt,
                ReplacedByToken = entity.ReplacedByToken
            };
        }

        public async Task CreateAsync(string userId, string token, DateTime expiresAt)
        {
            var entity = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt
            };

            _refreshTokenRepository.Create(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RevokeAsync(string refreshToken)
        {
            var entity = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);

            if (entity == null)
            {
                return;
            }

            entity.IsRevoked = true;
            _refreshTokenRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RevokeAllForAsync(string userId)
        {
            var entities = await _refreshTokenRepository.GetAllByUserIdAsync(userId);
            foreach (var entity in entities)
            {
                entity.IsRevoked = true;
                _refreshTokenRepository.Update(entity);
            }
            await _unitOfWork.SaveAsync();
        }
    }
}