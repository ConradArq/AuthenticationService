using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Delete
{
    public class DeleteApplicationMenuCommand: IRequest<ResponseDto<object>>
    {
        public int Id { get; set; }
    }
}
