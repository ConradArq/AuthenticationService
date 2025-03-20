using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Commands.Update
{
    public class UpdateApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand, ResponseDto<ApplicationUserResponse>>
    {
        private IApplicationUserService _applicationUserService;

        public UpdateApplicationUserCommandHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.UpdateAsync(request.Id, request);
            return result;
        }
    }
}
