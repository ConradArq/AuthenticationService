using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.Identity.Commands.ChangePassword;
using AuthenticationService.Application.Features.Identity.Commands.ConfirmEmail;
using AuthenticationService.Application.Features.Identity.Commands.ConfirmResetPassword;
using AuthenticationService.Application.Features.Identity.Commands.Register;
using AuthenticationService.Application.Features.Identity.Commands.ResetPassword;
using AuthenticationService.Application.Features.ApplicationUser;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<ResponseDto<ApplicationUserResponse>> RegisterAsync(RegisterCommand request);
        Task<ResponseDto<ApplicationUserResponse>> ConfirmEmailAsync(ConfirmEmailCommand request);
        Task<ResponseDto<object>> ChangePasswordAsync(ChangePasswordCommand request);
        Task<ResponseDto<object>> ResetPasswordAsync(ResetPasswordCommand request);
        Task<ResponseDto<object>> ConfirmResetPasswordAsync(ConfirmResetPasswordCommand request);
    }
}
