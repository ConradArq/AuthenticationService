using MediatR;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.TokenRefresh
{
    public class TokenRefreshCommandHandler : IRequestHandler<TokenRefreshCommand, ResponseDto<AuthenticationResponse>>
    {
        private IAuthenticationService _authenticationService;

        public TokenRefreshCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ResponseDto<AuthenticationResponse>> Handle(TokenRefreshCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.TokenRefreshAsync(request);
            return result;
        }
    }
}