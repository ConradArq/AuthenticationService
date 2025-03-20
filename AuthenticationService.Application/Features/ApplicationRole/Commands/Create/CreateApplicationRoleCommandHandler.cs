using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Create
{
    public class CreateApplicationRoleCommandHandler : IRequestHandler<CreateApplicationRoleCommand, ResponseDto<ApplicationRoleResponse>>
    {
        private IApplicationRoleService _applicationRoleService;

        public CreateApplicationRoleCommandHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<ApplicationRoleResponse>> Handle(CreateApplicationRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.CreateAsync(request);
            return result;
        }
    }
}
