using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.Status.Queries.GetAllPaginated
{
    public class GetAllPaginatedStatusQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ResponseStatus>>>
    {
    }
}
