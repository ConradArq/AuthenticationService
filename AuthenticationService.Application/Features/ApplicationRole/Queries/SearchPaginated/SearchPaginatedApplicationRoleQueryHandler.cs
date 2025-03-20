using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.SearchPaginated
{
    public class SearchPaginatedApplicationRoleQueryHandler : IRequestHandler<SearchPaginatedApplicationRoleQuery, PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
        private IApplicationRoleService _applicationRoleService;

        public SearchPaginatedApplicationRoleQueryHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>> Handle(SearchPaginatedApplicationRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.SearchPaginatedAsync(request);
            return result;
        }
    }
}
