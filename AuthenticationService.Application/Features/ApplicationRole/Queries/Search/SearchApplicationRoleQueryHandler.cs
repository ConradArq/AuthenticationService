using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.Search
{
    public class SearchApplicationRoleQueryHandler : IRequestHandler<SearchApplicationRoleQuery, ResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
        private IApplicationRoleService _applicationRoleService;

        public SearchApplicationRoleQueryHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationRoleResponse>>> Handle(SearchApplicationRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.SearchAsync(request);
            return result;
        }
    }
}
