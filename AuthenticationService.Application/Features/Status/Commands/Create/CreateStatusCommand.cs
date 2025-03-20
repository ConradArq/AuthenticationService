using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.Status.Commands.Create
{
    public class CreateStatusCommand: IRequest<ResponseDto<ResponseStatus>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int StatusId { get; set; } = (int)Domain.Enums.Status.Active;
    }
}
