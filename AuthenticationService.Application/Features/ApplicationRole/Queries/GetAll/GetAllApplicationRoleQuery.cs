using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.GetAll
{
    public class GetAllApplicationRoleQuery : IRequest<ResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
    }
}
