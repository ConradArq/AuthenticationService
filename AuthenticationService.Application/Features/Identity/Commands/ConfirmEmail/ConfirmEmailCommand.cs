using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using AuthenticationService.Application.Features.ApplicationUser;

namespace AuthenticationService.Application.Features.Identity.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand: IRequest<ResponseDto<ApplicationUserResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
