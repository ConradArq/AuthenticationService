using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthenticationService.Infrastructure.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using AuthenticationService.Shared.Dtos.ApplicationUser;
using AuthenticationService.Shared.Interfaces.Providers;
using AuthenticationService.Shared.Dtos.Authentication;
using AutoMapper;

namespace AuthenticationService.Infrastructure.Providers
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public JwtTokenProvider(IOptions<JwtSettings> jwtSettings, IServiceProvider serviceProvider, IMapper mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task<string> GenerateAuthenticationTokenAsync(object applicationUserId, TimeSpan? expirationTime = null, params Claim[] additionalClaims)
        {
            ApplicationUser? applicationUser = null;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                applicationUser = await userManager.FindByIdAsync((string)applicationUserId);
            }

            if (applicationUser == null) throw new ArgumentNullException(nameof(applicationUser));
            return await GenerateTokenInternalAsync(applicationUser: applicationUser, expirationTime: expirationTime, claims: additionalClaims);
        }

        public async Task<string> GenerateAuthenticationTokenAsync(string oldToken, TimeSpan? expirationTime = null, params Claim[] additionalClaims)
        {
            if (string.IsNullOrEmpty(oldToken)) throw new ArgumentNullException(nameof(oldToken));
            return await GenerateTokenInternalAsync(oldToken: oldToken, expirationTime: expirationTime, claims: additionalClaims);
        }

        public async Task<string> GenerateAuthenticationTokenAsync(TimeSpan? expirationTime = null, params Claim[] claims)
        {
            return await GenerateTokenInternalAsync(expirationTime: expirationTime, claims: claims);
        }

        public string? GetUserAuthenticationToken()
        {
            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var authorizationHeader = httpContextAccessor?.HttpContext?.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        public ApplicationUserResponseDto GetUserDataFromFromAuthenticationToken()
        {
            ApplicationUserResponseDto applicationUserResponseDto = new();

            var accessToken = GetUserAuthenticationToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("User authentication token is missing or invalid.");
            }

            ClaimsPrincipal claimsPrincipal = ExtractClaimsFromAuthenticationToken(accessToken);
            applicationUserResponseDto.Id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            applicationUserResponseDto.Email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            applicationUserResponseDto.UserName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

            return applicationUserResponseDto;
        }

        private async Task<string> GenerateTokenInternalAsync(
            TimeSpan? expirationTime = null,
            string? oldToken = null,
            ApplicationUser? applicationUser = null,
            params Claim[] claims)
        {
            var claimsList = new List<Claim>();

            // Extract claims from old token
            if (!string.IsNullOrEmpty(oldToken))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
                };

                try
                {
                    var principal = tokenHandler.ValidateToken(oldToken, validationParameters, out var validatedToken);
                    claimsList.AddRange(principal.Claims);
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid or expired token.", ex);
                }
            }

            // Extract claims from ApplicationUser
            if (applicationUser != null)
            {
                claimsList.AddRange(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
                    new Claim(ClaimTypes.Email, applicationUser.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, applicationUser.UserName ?? string.Empty)
                });

                using (var scope = _serviceProvider.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var userClaims = await userManager.GetClaimsAsync(applicationUser);
                    var roles = await userManager.GetRolesAsync(applicationUser);
                    var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

                    claimsList.AddRange(userClaims);
                    claimsList.AddRange(roleClaims);
                }
            }

            // Add provided claims
            if (claims.Any())
            {
                claimsList.AddRange(claims);
            }

            // Ensure required NameIdentifier claim exists
            if (!claimsList.Any(c => c.Type == ClaimTypes.NameIdentifier))
            {
                claimsList.Add(new Claim(ClaimTypes.NameIdentifier, "System"));
            }

            // Remove duplicate claims
            var uniqueClaims = claimsList
                .GroupBy(c => new { c.Type, c.Value })
                .Select(g => g.First())
                .ToList();

            // Generate token
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: uniqueClaims,
                expires: expirationTime.HasValue ? DateTime.UtcNow.Add(expirationTime.Value) : DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private ClaimsPrincipal ExtractClaimsFromAuthenticationToken(string token, bool validateLifetime = true)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = validateLifetime,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token.");
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid or expired token.", ex);
            }
        }

        JwtSettingsDto IJwtTokenProvider.GetJwtSettings()
        {
            var jwtSettingsDto = _mapper.Map<JwtSettingsDto>(_jwtSettings);
            return jwtSettingsDto;
        }
    }
}