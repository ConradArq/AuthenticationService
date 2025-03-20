using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.LoginVerifyOtp
{
    public class LoginVerifyOtpCommand: IRequest<ResponseDto<AuthenticationResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
