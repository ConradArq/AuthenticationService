using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.Get
{
    public class GetApplicationMenuQuery : IRequest<ResponseDto<ApplicationMenuResponse>>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
