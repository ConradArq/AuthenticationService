using AutoMapper;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.Authentication.Commands.Logout;
using AuthenticationService.Application.Features.Authentication.Commands.Login;
using AuthenticationService.Application.Features.Authentication;
using AuthenticationService.Application.Features.Authentication.Commands.TokenRefresh;
using AuthenticationService.Application.Features.Authentication.Commands.GoogleLogin;
using AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin;
using AuthenticationService.Shared.Interfaces.Services;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Shared.Resources;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models.Entities;
using Microsoft.Extensions.Configuration;
using AuthenticationService.Shared.Interfaces.Providers;
using System.Security.Cryptography;
using AuthenticationService.Application.Features.ApplicationUser;
using AuthenticationService.Application.Features.Identity.Commands.Register;
using AuthenticationService.Application.Features.Authentication.Commands;
using AuthenticationService.Application.Features.Authentication.Commands.LoginVerifyOtp;
using AuthenticationService.Shared.Interfaces.Factories;
using AuthenticationService.Shared.Dtos.Templates.Emails;

namespace AuthenticationService.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IOtpService _otpService;
        private readonly IEmailNotificationServiceFactory _emailNotificationServiceFactory;

        public AuthenticationService(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtTokenProvider jwtTokenProvider, IRefreshTokenService refreshTokenService, IConfiguration configuration, IUnitOfWork unitOfWork, IIdentityService identityService, IOtpService otpService, IEmailNotificationServiceFactory emailNotificationServiceFactory)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenProvider = jwtTokenProvider;
            _refreshTokenService = refreshTokenService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _otpService = otpService;
            _emailNotificationServiceFactory = emailNotificationServiceFactory;
        }

        public async Task<ResponseDto<AuthenticationResponse>> LoginAsync(LoginCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(string.Format(AuthMessages.EmailNotFoundMessage, request.Email));
            }

            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedAccessException(string.Format(AuthMessages.EmailNotVerifiedMessage, request.Email));
            }

            var authUser = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!authUser.Succeeded)
            {
                throw new UnauthorizedAccessException(AuthMessages.FailedLoginMessage);
            }

            if (user.TwoFactorEnabled)
            {
                #region Send Email

                IUserEmailService<object> notificationService = _emailNotificationServiceFactory.Create<TwoFactorOtpEmailDto>();
                notificationService.SendEmail(user.Id);

                #endregion

                return new ResponseDto<AuthenticationResponse>(new AuthenticationResponse(), AuthMessages.TwoFactorOtpVerificationPendingMessage);
            }

            var response = await GenerateLoginResponseAsync(user);
            return response;
        }

        public async Task<ResponseDto<AuthenticationResponse>> LoginVerifyOtpAsync(LoginVerifyOtpCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(string.Format(AuthMessages.EmailNotFoundMessage, request.Email));
            }

            var isOtpValid = await _otpService.ValidateOtpAsync(user.Id, request.Otp);

            if (!isOtpValid)
            {
                throw new BadRequestException(AuthMessages.InvalidOrExpiredOTPMessage);
            }

            var response = await GenerateLoginResponseAsync(user);
            return response;
        }

        public async Task<ResponseDto<AuthenticationResponse>> GoogleLoginAsync(GoogleLoginCommand request)
        {
            // The entity must be tracked to prevent duplication errors if an update is needed.
            var user = (await _unitOfWork.ApplicationUserRepository.GetAsync(x => x.GoogleId == request.GoogleId, disableTracking : false)).FirstOrDefault();
            var response = await LoginOAuthAsync(user, request);
            return response;
        }

        public async Task<ResponseDto<AuthenticationResponse>> MicrosoftLoginAsync(MicrosoftLoginCommand request)
        {
            // The entity must be tracked to prevent duplication errors if an update is needed.
            var user = (await _unitOfWork.ApplicationUserRepository.GetAsync(x => x.MicrosoftId == request.MicrosoftId, disableTracking: false)).FirstOrDefault();
            var response = await LoginOAuthAsync(user, request);
            return response;
        }

        public async Task<ResponseDto<object>> LogoutAsync(LogoutCommand request)
        {
            var userResponseDto = _jwtTokenProvider.GetUserDataFromFromAuthenticationToken();

            var refreshTokenDto = await _refreshTokenService.GetNonRevokedByAsync(userResponseDto.Id);

            if (refreshTokenDto != null)
            {
                await _refreshTokenService.RevokeAsync(refreshTokenDto.Token);
            }

            return new ResponseDto<object>();
        }

        public async Task<ResponseDto<AuthenticationResponse>> TokenRefreshAsync(TokenRefreshCommand request)
        {
            var storedRefreshToken = await _refreshTokenService.GetByAsync(request.RefreshToken);

            if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException(AuthMessages.InvalidRefreshTokenMessage);
            }

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);

            if (user == null)
            {
                throw new Exception($"User with 'Id' {storedRefreshToken.UserId} cannot be found for refresh token {storedRefreshToken}");
            }

            var response = await GenerateLoginResponseAsync(user);
            return response;
        }

        private async Task<ResponseDto<AuthenticationResponse>> LoginOAuthAsync(ApplicationUser? user, LoginOAuthCommand request)
        {
            var existingUserWithEmail = await _userManager.FindByEmailAsync(request.Email);

            // If the user is not found by their unique OAuth provider ID (user passed as null), we check if another user exists
            // with the email sent in the request. If it does not exist, we create the user with the default role (or "Guest" if not
            // configured) and mark them as unregistered. If it does exist, the user must choose another OAuth account to proceed.
            if (user == null)
            {
                if(existingUserWithEmail == null)
                {
                    await RegisterOAuthUserAsync(request);
                }
                else
                {
                    throw new ConflictException(string.Format(AuthMessages.UserWithEmailAlreadyExistsMessage, request.Email));
                }
            }
            // If the user is found by their unique OAuth provider ID and has not completed the standard
            // registration process, but their email has changed in the OAuth provider, we update it in our database as well.
            else
            {
                await UpdateOAuthUserEmailIfNecessaryAsync(user, request.Email);
            }

            // Once the user is created, we log them in to generate a custom bearer token. 
            // Since they are assigned the default role (or "Guest" if not configured), they can log in with the token 
            // but will have limited permissions, as enforced by the API based on the token's role and claims.

            user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(string.Format(AuthMessages.EmailNotFoundMessage, request.Email));
            }

            var response = await GenerateLoginResponseAsync(user);
            return response;
        }

        private async Task RegisterOAuthUserAsync(LoginOAuthCommand request)
        {
            var registerCommand = _mapper.Map<RegisterCommand>(request);
            registerCommand.RoleNames.Add(_configuration["Identity:DefaultRole"] ?? "Guest");
            registerCommand.IsRegistered = false;
            registerCommand.EmailConfirmed = true;

            await _identityService.RegisterAsync(registerCommand);
        }

        private async Task UpdateOAuthUserEmailIfNecessaryAsync(ApplicationUser user, string newEmail)
        {
            if (!user.IsRegistered && user.Email != newEmail)
            {
                user.Email = newEmail;
                var (isSuccess, errors) = await _unitOfWork.ApplicationUserRepository.UpdateUserAsync(user);
                if (!isSuccess)
                {
                    throw new InvalidOperationException($"Failed to update user email: {string.Join(" ", errors)}");
                }
            }
        }

        private async Task<ResponseDto<AuthenticationResponse>> GenerateLoginResponseAsync(ApplicationUser user)
        {
            var roleNames = await _userManager.GetRolesAsync(user);

            if (roleNames.Count <= 0)
            {
                throw new ForbiddenException(string.Format(AuthMessages.UserWithNoRoleMessage, user.Email));
            }

            var accessToken = await _jwtTokenProvider.GenerateAuthenticationTokenAsync((object)user.Id);

            var refreshToken = GenerateSecureRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(7);

            await _refreshTokenService.RevokeAllForAsync(user.Id);
            await _refreshTokenService.CreateAsync(user.Id, refreshToken, refreshTokenExpirationDate);

            var applicationUserResponse = _mapper.Map<ApplicationUserResponse>(user);
            applicationUserResponse.RoleNames = roleNames.ToList();

            var response = new ResponseDto<AuthenticationResponse>(new AuthenticationResponse
            {
                AccessToken = accessToken,
                AccessTokenExpirationDate = DateTime.UtcNow.AddMinutes(_jwtTokenProvider.GetJwtSettings().DurationInMinutes),
                RefreshToken = refreshToken,
                RefreshTokenExpirationDate = refreshTokenExpirationDate,
                User = applicationUserResponse
            });

            return response;
        }

        private string GenerateSecureRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
