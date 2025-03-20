using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Delete
{
    public class DeleteApplicationMenuCommand: IRequest<ResponseDto<object>>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
