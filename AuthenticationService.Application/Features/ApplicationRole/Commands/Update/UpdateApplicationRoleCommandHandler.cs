using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Update
{
    public class UpdateApplicationRoleCommandHandler : IRequestHandler<UpdateApplicationRoleCommand, ResponseDto<ApplicationRoleResponse>>
    {
        private IApplicationRoleService _applicationRoleService;

        public UpdateApplicationRoleCommandHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<ApplicationRoleResponse>> Handle(UpdateApplicationRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.UpdateAsync(request.Id, request);
            return result;
        }
    }
}
