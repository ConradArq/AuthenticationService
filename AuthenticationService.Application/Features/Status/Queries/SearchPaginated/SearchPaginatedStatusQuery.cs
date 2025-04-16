using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Status.Queries.SearchPaginated
{
    public class SearchPaginatedStatusQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ResponseStatus>>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
