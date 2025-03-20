using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole
{
    public class RemoveMenusFromRoleCommand: IRequest<ResponseDto<ApplicationRoleMenuResponse>>
    {
        [JsonIgnore]
        public string ApplicationRoleId { get; set; } = string.Empty;
        public List<int> ApplicationMenuIds { get; set; } = new List<int>();
    }
}
