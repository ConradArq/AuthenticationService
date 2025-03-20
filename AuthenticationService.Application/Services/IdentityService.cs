using AuthenticationService.Application.Features.Identity.Commands.ChangePassword;
using AuthenticationService.Application.Features.Identity.Commands.ConfirmEmail;
using AuthenticationService.Application.Features.Identity.Commands.ConfirmResetPassword;
using AuthenticationService.Application.Features.Identity.Commands.Register;
using AuthenticationService.Application.Features.Identity.Commands.ResetPassword;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;
using AutoMapper;
using AuthenticationService.Application.Features.ApplicationUser;
using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Shared.Interfaces.Providers;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Shared.Interfaces.Factories;
using AuthenticationService.Shared.Dtos.Templates.Emails;
using AuthenticationService.Shared.Exceptions;
using AuthenticationService.Shared.Resources;
using System.Net;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Update;
using AuthenticationService.Shared.Interfaces.Services;

namespace AuthenticationService.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IEmailNotificationServiceFactory _emailNotificationServiceFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;

        public IdentityService(IMapper mapper, UserManager<ApplicationUser> userManager, IJwtTokenProvider jwtTokenProvider, IEmailNotificationServiceFactory emailNotificationServiceFactory, IUnitOfWork unitOfWork, IApplicationUserService applicationUserService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
            _emailNotificationServiceFactory = emailNotificationServiceFactory;
            _unitOfWork = unitOfWork;
            _applicationUserService = applicationUserService;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> RegisterAsync(RegisterCommand request)
        {
            #region Check if User Exists and Handle Existing Users Created via OAuth Flow

            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingUserByEmail != null)
            {
                // If the user was created through an OAuth flow, they will be marked as not registered. In this case we update
                // the user's data with the information from the request command, which will automatically mark them as registered
                // (IsRegistered defaults to true in RegisterCommand). Then, a welcome email is sent.
                if (!existingUserByEmail.IsRegistered)
                {
                    var updateApplicationUserCommand = _mapper.Map<UpdateApplicationUserCommand>(request);
                    updateApplicationUserCommand.Id = existingUserByEmail.Id;
                    var updateResponse = await _applicationUserService.UpdateAsync(existingUserByEmail.Id, updateApplicationUserCommand);

                    var result = await _userManager.AddPasswordAsync(existingUserByEmail, request.Password);
                    if (!result.Succeeded)
                    {
                        var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Password addition failed: {errorMessages}");
                    }

                    #region Send Email

                    var welcomeEmailNotificationService = _emailNotificationServiceFactory.Create<WelcomeEmailDto>();
                    welcomeEmailNotificationService.SendEmail(existingUserByEmail.Id);

                    #endregion

                    return updateResponse;
                }
                // If the user exists and has already been registered through the standard flow, we throw exception.
                else
                {
                    throw new ConflictException(string.Format(AuthMessages.UserWithEmailAlreadyExistsMessage, request.Email));
                }
            }

            if (request.UserName != null)
            {
                var existingUserByUserName = await _userManager.FindByNameAsync(request.UserName);
                if (existingUserByUserName != null)
                {
                    throw new ConflictException(string.Format(AuthMessages.UserWithUserNameAlreadyExistsMessage, request.UserName));
                }
            }

            #endregion

            #region Create User & Assign Roles (Transactional)

            ApplicationUser user = _mapper.Map<ApplicationUser>(request);

            await _unitOfWork.CompleteTransactionAsync(async () =>
            {
                // Username is enforced by .NET Identity, so we ensure it is set if not provided.
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    user.UserName = await GenerateUniqueUsernameFromEmailAsync(user.Email);
                }

                // Create user without a password if coming from the OAuth flow, 
                // or with a password if coming from the standard registration flow.
                var createUserResult = string.IsNullOrEmpty(request.Password)
                    ? await _userManager.CreateAsync(user)
                    : await _userManager.CreateAsync(user, request.Password);

                if (!createUserResult.Succeeded)
                {
                    throw new Exception(string.Join(" ", createUserResult.Errors.Select(error => error.Description)));
                }

                // Validate roles before adding
                foreach (var roleName in request.RoleNames)
                {
                    if (!await _unitOfWork.ApplicationRoleRepository.RoleExistsAsync(roleName))
                    {
                        throw new NotFoundException(string.Format(AuthMessages.RoleNotFoundMessage, roleName));
                    }
                }

                // Assign roles
                var addRolesResult = await _userManager.AddToRolesAsync(user, request.RoleNames);
                if (!addRolesResult.Succeeded)
                {
                    throw new Exception(string.Join(" ", addRolesResult.Errors.Select(error => error.Description)));
                }
            });

            #endregion

            #region Send Email

            IUserEmailService<object> notificationService = user.EmailConfirmed
                ? _emailNotificationServiceFactory.Create<WelcomeEmailDto>()
                : _emailNotificationServiceFactory.Create<EmailConfirmationEmailDto>();

            notificationService.SendEmail(user.Id);

            #endregion

            var applicationUserResponse = _mapper.Map<ApplicationUserResponse>(user);
            applicationUserResponse.RoleNames = request.RoleNames;

            var response = new ResponseDto<ApplicationUserResponse>(applicationUserResponse, AuthMessages.AccountCreationEmailConfirmationPendingMessage);
            return response;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> ConfirmEmailAsync(ConfirmEmailCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(string.Format(AuthMessages.EmailNotFoundMessage, request.Email));
            }

            if (!user.EmailConfirmed)
            {
                var decodedToken = WebUtility.UrlDecode(request.Token);
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (!result.Succeeded)
                {
                    var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));

                    if (result.Errors.Any(e => e.Code == "InvalidToken"))
                    {
                        throw new BadRequestException(AuthMessages.InvalidEmailConfirmationTokenMessage);
                    }

                    throw new Exception($"Email confirmation failed: {errorMessages}");
                }

                #region Send Email

                var notificationService = _emailNotificationServiceFactory.Create<WelcomeEmailDto>();
                notificationService.SendEmail(user.Id);

                #endregion
            }

            var applicationUserResponse = _mapper.Map<ApplicationUserResponse>(user);
            var userRoleNames = await _userManager.GetRolesAsync(user);
            applicationUserResponse.RoleNames = userRoleNames.ToList();

            var response = new ResponseDto<ApplicationUserResponse>(applicationUserResponse);

            return response;
        }

        public async Task<ResponseDto<object>> ChangePasswordAsync(ChangePasswordCommand request)
        {
            var userResponseDto = _jwtTokenProvider.GetUserDataFromFromAuthenticationToken();

            var user = await _userManager.FindByIdAsync(userResponseDto.Id);
            if (user == null)
            {
                throw new Exception($"The user with Id {userResponseDto.Id} does not exist.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException(AuthMessages.InvalidCurrentPasswordMessage);
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            return new ResponseDto<object>();
        }

        public async Task<ResponseDto<object>> ResetPasswordAsync(ResetPasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(string.Format(AuthMessages.EmailNotFoundMessage, request.Email));
            }

            #region Send Email

            var notificationService = _emailNotificationServiceFactory.Create<PasswordResetEmailDto>();
            notificationService.SendEmail(user.Id);

            #endregion

            return new ResponseDto<object>(AuthMessages.PasswordResetEmailSentMessage);
        }

        public async Task<ResponseDto<object>> ConfirmResetPasswordAsync(ConfirmResetPasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(string.Format(AuthMessages.EmailNotFoundMessage, request.Email));
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));

                if (result.Errors.Any(e => e.Code == "InvalidToken"))
                {
                    throw new BadRequestException(AuthMessages.InvalidPasswordResetTokenMessage);
                }

                throw new InvalidOperationException($"Password reset failed: {errorMessages}");
            }

            return new ResponseDto<object>();
        }

        private async Task<string> GenerateUniqueUsernameFromEmailAsync(string email)
        {
            var baseUsername = email?.Split('@')[0] ?? Guid.NewGuid().ToString();

            if (await _userManager.FindByNameAsync(baseUsername) == null)
                return baseUsername;

            return $"{baseUsername}_{Guid.NewGuid().ToString("N").Substring(6)}";
        }
    }
}
