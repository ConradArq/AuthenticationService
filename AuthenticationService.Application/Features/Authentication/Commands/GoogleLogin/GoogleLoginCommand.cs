using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Authentication.Commands.GoogleLogin
{
    public class GoogleLoginCommand: LoginOAuthCommand, IRequest<ResponseDto<AuthenticationResponse>>
    {
        public string GoogleId { get; set; } = string.Empty;
    }
}
