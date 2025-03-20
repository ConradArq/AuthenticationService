using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Identity.Commands.ConfirmResetPassword
{
    public class ConfirmResetPasswordCommandHandler : IRequestHandler<ConfirmResetPasswordCommand, ResponseDto<object>>
    {
        private IIdentityService _identityService;

        public ConfirmResetPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseDto<object>> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ConfirmResetPasswordAsync(request);
            return result;
        }
    }
}
