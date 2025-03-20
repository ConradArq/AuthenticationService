using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.Delete
{
    public class DeleteApplicationRoleCommandHandler : IRequestHandler<DeleteApplicationRoleCommand, ResponseDto<object>>
    {
        private IApplicationRoleService _applicationRoleService;

        public DeleteApplicationRoleCommandHandler(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public async Task<ResponseDto<object>> Handle(DeleteApplicationRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationRoleService.DeleteAsync(request.Id);
            return result;
        }
    }
}
