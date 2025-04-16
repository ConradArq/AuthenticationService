using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.GetAll
{
    public class GetAllApplicationRoleQuery : RequestDto, IRequest<ResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
    }
}
