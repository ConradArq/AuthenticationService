using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.Search
{
    public class SearchApplicationMenuQueryHandler : IRequestHandler<SearchApplicationMenuQuery, ResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
        private IApplicationMenuService _applicationMenuService;

        public SearchApplicationMenuQueryHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationMenuResponse>>> Handle(SearchApplicationMenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.SearchAsync(request);
            return result;
        }
    }
}
