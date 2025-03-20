using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.SearchPaginated
{
    public class SearchPaginatedApplicationUserQueryHandler : IRequestHandler<SearchPaginatedApplicationUserQuery, PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
        private IApplicationUserService _applicationUserService;

        public SearchPaginatedApplicationUserQueryHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>> Handle(SearchPaginatedApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.SearchPaginatedAsync(request);
            return result;
        }
    }
}
