using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.Status.Commands.Update
{
    public class UpdateStatusCommand : IRequest<ResponseDto<ResponseStatus>>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int? StatusId { get; set; }
    }
}
