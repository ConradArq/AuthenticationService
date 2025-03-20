using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Create
{
    public class CreateApplicationRoleCommand: IRequest<ResponseDto<ApplicationRoleResponse>>
    {
        public string Name { get; set; } = string.Empty;

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int StatusId { get; set; } = (int)Domain.Enums.Status.Active;
    }
}
