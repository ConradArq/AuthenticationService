using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAllPaginated
{
    public class GetAllPaginatedApplicationMenuQueryHandler : IRequestHandler<GetAllPaginatedApplicationMenuQuery, PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
        private IApplicationMenuService _applicationMenuService;

        public GetAllPaginatedApplicationMenuQueryHandler(IApplicationMenuService applicationMenuService)
        {
            _applicationMenuService = applicationMenuService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>> Handle(GetAllPaginatedApplicationMenuQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationMenuService.GetAllPaginatedAsync(request);
            return result;
        }
    }
}
