using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.Get
{
    public class GetApplicationRoleQueryHandler : IRequestHandler<GetApplicationRoleQuery, ResponseDto<ApplicationRoleResponse>>
    {
        private IApplicationRoleService _applicationRoleService;

        public GetApplicationRoleQueryHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<ApplicationRoleResponse>> Handle(GetApplicationRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.GetAsync(request.Id);
            return result;
        }
    }
}
