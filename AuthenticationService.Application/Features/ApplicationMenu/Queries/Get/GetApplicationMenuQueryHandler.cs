using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.Get
{
    public class GetApplicationMenuQueryHandler : IRequestHandler<GetApplicationMenuQuery, ResponseDto<ApplicationMenuResponse>>
    {
        private IApplicationMenuService _applicationMenuService;

        public GetApplicationMenuQueryHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<ResponseDto<ApplicationMenuResponse>> Handle(GetApplicationMenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.GetAsync(request.Id);
            return result;
        }
    }
}
