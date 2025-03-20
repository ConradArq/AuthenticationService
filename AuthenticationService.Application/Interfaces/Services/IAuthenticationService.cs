using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.Authentication.Commands.Login;
using AuthenticationService.Application.Features.Authentication.Commands.Logout;
using AuthenticationService.Application.Features.Authentication;
using AuthenticationService.Application.Features.Authentication.Commands.TokenRefresh;
using AuthenticationService.Application.Features.Authentication.Commands.GoogleLogin;
using AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin;
using AuthenticationService.Application.Features.Authentication.Commands.LoginVerifyOtp;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseDto<AuthenticationResponse>> LoginAsync(LoginCommand request);
        Task<ResponseDto<AuthenticationResponse>> LoginVerifyOtpAsync(LoginVerifyOtpCommand request);
        Task<ResponseDto<AuthenticationResponse>> GoogleLoginAsync(GoogleLoginCommand request);
        Task<ResponseDto<AuthenticationResponse>> MicrosoftLoginAsync(MicrosoftLoginCommand request);
        Task<ResponseDto<object>> LogoutAsync(LogoutCommand request);
        Task<ResponseDto<AuthenticationResponse>> TokenRefreshAsync(TokenRefreshCommand request);
    }
}
