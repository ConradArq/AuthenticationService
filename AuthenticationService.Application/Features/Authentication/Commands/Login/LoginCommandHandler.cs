using MediatR;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResponseDto<AuthenticationResponse>>
    {
        private IAuthenticationService _authenticationService;

        public LoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ResponseDto<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginAsync(request);
            return result;
        }
    }
}
