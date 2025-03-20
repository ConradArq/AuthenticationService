using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.GetAllPaginated
{
    public class GetAllPaginatedApplicationUserQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
    }
}
