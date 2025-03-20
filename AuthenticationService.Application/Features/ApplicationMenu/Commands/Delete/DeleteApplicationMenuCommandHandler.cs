using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Delete
{
    public class DeleteApplicationMenuCommandHandler : IRequestHandler<DeleteApplicationMenuCommand, ResponseDto<object>>
    {
        private IApplicationMenuService _applicationMenuService;

        public DeleteApplicationMenuCommandHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<ResponseDto<object>> Handle(DeleteApplicationMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.DeleteAsync(request.Id);
            return result;
        }
    }
}
