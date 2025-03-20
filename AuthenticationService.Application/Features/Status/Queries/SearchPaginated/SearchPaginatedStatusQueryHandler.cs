using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Queries.SearchPaginated
{
    public class SearchPaginatedStatusQueryHandler : IRequestHandler<SearchPaginatedStatusQuery, PaginatedResponseDto<IEnumerable<ResponseStatus>>>
    {
        private IStatusService _statusService;

        public SearchPaginatedStatusQueryHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ResponseStatus>>> Handle(SearchPaginatedStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _statusService.SearchPaginatedAsync(request);
            return result;
        }
    }
}
