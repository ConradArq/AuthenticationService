using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthenticationService.Application.Features.Authentication.Commands.TokenRefresh
{
    public class TokenRefreshCommand : IRequest<ResponseDto<AuthenticationResponse>>
    {
        [SwaggerSchema(Description = "The refresh token issued to the client. This is used to generate a new access token.")]
        [DefaultValue("")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
