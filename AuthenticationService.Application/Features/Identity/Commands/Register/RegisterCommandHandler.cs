using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Application.Features.ApplicationUser;

namespace AuthenticationService.Application.Features.Identity.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResponseDto<ApplicationUserResponse>>
    {
        private IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(request);
            return result;
        }
    }
}
