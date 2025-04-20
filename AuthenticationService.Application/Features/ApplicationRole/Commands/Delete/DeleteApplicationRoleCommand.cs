using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Delete
{
    public class DeleteApplicationRoleCommand: IRequest<ResponseDto<object>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
