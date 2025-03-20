using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin
{
    public class MicrosoftLoginCommand: LoginOAuthCommand, IRequest<ResponseDto<AuthenticationResponse>>
    {
        public string MicrosoftId { get; set; } = string.Empty;
    }
}
