using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.Status.Queries.Get
{
    public class GetStatusQuery : IRequest<ResponseDto<ResponseStatus>>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
