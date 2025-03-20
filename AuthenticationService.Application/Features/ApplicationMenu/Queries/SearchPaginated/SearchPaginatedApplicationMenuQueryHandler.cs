using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.SearchPaginated
{
    public class SearchPaginatedApplicationMenuQueryHandler : IRequestHandler<SearchPaginatedApplicationMenuQuery, PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
        private IApplicationMenuService _applicationMenuService;

        public SearchPaginatedApplicationMenuQueryHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>> Handle(SearchPaginatedApplicationMenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.SearchPaginatedAsync(request);
            return result;
        }
    }
}
