using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Identity.Commands.ResetPassword
{
    public class ResetPasswordCommand: IRequest<ResponseDto<object>>
    {
        public string Email { get; set; } = string.Empty;
    }
}
