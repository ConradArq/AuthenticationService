using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.GetAllPaginated
{
    public class GetAllPaginatedApplicationRoleQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
    }
}
