using MediatR;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin
{
    public class MicrosoftLoginCommandHandler : IRequestHandler<MicrosoftLoginCommand, ResponseDto<AuthenticationResponse>>
    {
        private IAuthenticationService _authenticationService;

        public MicrosoftLoginCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ResponseDto<AuthenticationResponse>> Handle(MicrosoftLoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.MicrosoftLoginAsync(request);
            return result;
        }
    }
}
