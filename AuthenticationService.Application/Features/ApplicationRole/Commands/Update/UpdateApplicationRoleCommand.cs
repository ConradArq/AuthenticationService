using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Update
{
    public class UpdateApplicationRoleCommand : IRequest<ResponseDto<ApplicationRoleResponse>>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        public string? Name { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int? StatusId { get; set; }
    }
}
