using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.Get
{
    public class GetApplicationRoleQuery : IRequest<ResponseDto<ApplicationRoleResponse>>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
    }
}
