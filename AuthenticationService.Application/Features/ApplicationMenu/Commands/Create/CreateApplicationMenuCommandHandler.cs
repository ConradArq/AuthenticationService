using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Create
{
    public class CreateApplicationMenuCommandHandler : IRequestHandler<CreateApplicationMenuCommand, ResponseDto<ApplicationMenuResponse>>
    {
        private IApplicationMenuService _applicationMenuService;

        public CreateApplicationMenuCommandHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<ResponseDto<ApplicationMenuResponse>> Handle(CreateApplicationMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.CreateAsync(request);
            return result;
        }
    }
}
