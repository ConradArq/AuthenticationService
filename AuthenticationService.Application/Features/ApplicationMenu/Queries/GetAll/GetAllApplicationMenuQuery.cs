using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAll
{
    public class GetAllApplicationMenuQuery : RequestDto, IRequest<ResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
    }
}
