using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.GetAll
{
    public class GetAllApplicationRoleQueryHandler : IRequestHandler<GetAllApplicationRoleQuery, ResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
        private IApplicationRoleService _applicationRoleService;

        public GetAllApplicationRoleQueryHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationRoleResponse>>> Handle(GetAllApplicationRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.GetAllAsync(request);
            return result;
        }
    }
}
