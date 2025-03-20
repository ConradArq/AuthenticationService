using MediatR;
using AuthenticationService.Shared.Dtos;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.AssignMenusToRole
{
    public class AssignMenusToRoleCommand: IRequest<ResponseDto<ApplicationRoleMenuResponse>>
    {
        [JsonIgnore]
        public string ApplicationRoleId { get; set; } = string.Empty;
        public List<int> ApplicationMenuIds { get; set; } = new List<int>();
    }
}
