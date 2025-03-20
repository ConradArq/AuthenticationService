using MediatR;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ResponseDto<object>>
    {
        private IAuthenticationService _authenticationService;

        public LogoutCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ResponseDto<object>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LogoutAsync(request);
            return result;
        }
    }
}
