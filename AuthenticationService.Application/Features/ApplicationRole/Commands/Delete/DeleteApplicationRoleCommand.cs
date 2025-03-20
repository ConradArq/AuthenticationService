using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Delete
{
    public class DeleteApplicationRoleCommand: IRequest<ResponseDto<object>>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
    }
}
