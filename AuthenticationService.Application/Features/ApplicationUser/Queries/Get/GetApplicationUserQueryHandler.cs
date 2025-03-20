using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.Get
{
    public class GetApplicationUserQueryHandler : IRequestHandler<GetApplicationUserQuery, ResponseDto<ApplicationUserResponse>>
    {
        private IApplicationUserService _applicationUserService;

        public GetApplicationUserQueryHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<ResponseDto<ApplicationUserResponse>> Handle(GetApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.GetAsync(request.Id);
            return result;
        }
    }
}
