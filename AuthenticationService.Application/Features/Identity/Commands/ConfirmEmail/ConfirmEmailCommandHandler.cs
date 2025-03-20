using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;
using AuthenticationService.Application.Features.ApplicationUser;

namespace AuthenticationService.Application.Features.Identity.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ResponseDto<ApplicationUserResponse>>
    {
        private IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ConfirmEmailAsync(request);
            return result;
        }
    }
}
