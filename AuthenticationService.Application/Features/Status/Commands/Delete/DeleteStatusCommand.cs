using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.Status.Commands.Delete
{
    public class DeleteStatusCommand: IRequest<ResponseDto<object>>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
