using MediatR;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.LoginVerifyOtp
{
    public class LoginVerifyOtpCommandHandler : IRequestHandler<LoginVerifyOtpCommand, ResponseDto<AuthenticationResponse>>
    {
        private IAuthenticationService _authenticationService;

        public LoginVerifyOtpCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ResponseDto<AuthenticationResponse>> Handle(LoginVerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginVerifyOtpAsync(request);
            return result;
        }
    }
}
