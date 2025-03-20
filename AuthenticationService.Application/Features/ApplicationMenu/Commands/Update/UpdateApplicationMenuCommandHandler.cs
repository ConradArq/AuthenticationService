using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Update
{
    public class UpdateApplicationMenuCommandHandler : IRequestHandler<UpdateApplicationMenuCommand, ResponseDto<ApplicationMenuResponse>>
    {
        private IApplicationMenuService _applicationMenuService;

        public UpdateApplicationMenuCommandHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<ResponseDto<ApplicationMenuResponse>> Handle(UpdateApplicationMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.UpdateAsync(request.Id, request);
            return result;
        }
    }
}
