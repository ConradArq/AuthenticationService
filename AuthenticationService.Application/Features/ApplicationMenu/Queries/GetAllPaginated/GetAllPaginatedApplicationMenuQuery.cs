using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAllPaginated
{
    public class GetAllPaginatedApplicationMenuQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
    }
}
