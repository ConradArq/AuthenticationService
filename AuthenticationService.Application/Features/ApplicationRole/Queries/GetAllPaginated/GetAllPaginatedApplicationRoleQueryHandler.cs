using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.GetAllPaginated
{
    public class GetAllPaginatedApplicationRoleQueryHandler : IRequestHandler<GetAllPaginatedApplicationRoleQuery, PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
        private IApplicationRoleService _applicationRoleService;

        public GetAllPaginatedApplicationRoleQueryHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>> Handle(GetAllPaginatedApplicationRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.GetAllPaginatedAsync(request);
            return result;
        }
    }
}
