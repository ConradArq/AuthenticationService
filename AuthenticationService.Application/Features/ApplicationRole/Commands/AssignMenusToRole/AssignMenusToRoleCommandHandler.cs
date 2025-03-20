using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.AssignMenusToRole
{
    public class AssignMenusToRoleCommandHandler : IRequestHandler<AssignMenusToRoleCommand, ResponseDto<ApplicationRoleMenuResponse>>
    {
        private IApplicationRoleService _applicationRoleService;

        public AssignMenusToRoleCommandHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<ApplicationRoleMenuResponse>> Handle(AssignMenusToRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.AssignMenusToRoleAsync(request);
            return result;
        }
    }
}
