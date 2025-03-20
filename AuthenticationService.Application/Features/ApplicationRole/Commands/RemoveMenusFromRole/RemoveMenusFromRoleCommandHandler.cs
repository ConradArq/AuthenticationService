using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole
{
    public class RemoveMenusFromRoleCommandHandler : IRequestHandler<RemoveMenusFromRoleCommand, ResponseDto<ApplicationRoleMenuResponse>>
    {
        private IApplicationRoleService _applicationRoleService;

        public RemoveMenusFromRoleCommandHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<ApplicationRoleMenuResponse>> Handle(RemoveMenusFromRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.RemoveMenusFromRoleAsync(request);
            return result;
        }
    }
}
