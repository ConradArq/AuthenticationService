using MediatR;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, ResponseDto<AuthenticationResponse>>
    {
        private IAuthenticationService _authenticationService;

        public GoogleLoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ResponseDto<AuthenticationResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.GoogleLoginAsync(request);
            return result;
        }
    }
}
