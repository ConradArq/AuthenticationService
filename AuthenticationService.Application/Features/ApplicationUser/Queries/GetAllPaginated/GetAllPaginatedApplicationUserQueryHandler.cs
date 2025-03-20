using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.GetAllPaginated
{
    public class GetAllPaginatedApplicationUserQueryHandler : IRequestHandler<GetAllPaginatedApplicationUserQuery, PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
        private IApplicationUserService _applicationUserService;

        public GetAllPaginatedApplicationUserQueryHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>> Handle(GetAllPaginatedApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.GetAllPaginatedAsync(request);
            return result;
        }
    }
}
