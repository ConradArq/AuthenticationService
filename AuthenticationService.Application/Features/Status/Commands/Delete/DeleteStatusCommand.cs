using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Status.Commands.Delete
{
    public class DeleteStatusCommand: IRequest<ResponseDto<object>>
    {
        public int Id { get; set; }
    }
}
