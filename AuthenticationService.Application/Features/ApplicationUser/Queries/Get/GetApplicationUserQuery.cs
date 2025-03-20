using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.Get
{
    public class GetApplicationUserQuery : IRequest<ResponseDto<ApplicationUserResponse>>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
    }
}
