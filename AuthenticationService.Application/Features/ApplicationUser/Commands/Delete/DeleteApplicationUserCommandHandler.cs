using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Commands.Delete
{
    public class DeleteApplicationUserCommandHandler : IRequestHandler<DeleteApplicationUserCommand, ResponseDto<object>>
    {
        private IApplicationUserService _applicationUserService;

        public DeleteApplicationUserCommandHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<ResponseDto<object>> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.DeleteAsync(request.Id);
            return result;
        }
    }
}
