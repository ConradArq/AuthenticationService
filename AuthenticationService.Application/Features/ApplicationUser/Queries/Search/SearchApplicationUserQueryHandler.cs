using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.Search
{
    public class SearchApplicationUserQueryHandler : IRequestHandler<SearchApplicationUserQuery, ResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
        private IApplicationUserService _applicationUserService;

        public SearchApplicationUserQueryHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationUserResponse>>> Handle(SearchApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.SearchAsync(request);
            return result;
        }
    }
}
