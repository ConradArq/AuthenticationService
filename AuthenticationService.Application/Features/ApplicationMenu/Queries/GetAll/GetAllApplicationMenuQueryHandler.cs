using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAll
{
    public class GetAllApplicationMenuQueryHandler : IRequestHandler<GetAllApplicationMenuQuery, ResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
        private IApplicationMenuService _applicationMenuService;

        public GetAllApplicationMenuQueryHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationMenuResponse>>> Handle(GetAllApplicationMenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.GetAllAsync(request);
            return result;
        }
    }
}
