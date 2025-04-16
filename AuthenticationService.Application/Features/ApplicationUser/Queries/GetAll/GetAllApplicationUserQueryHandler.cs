using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.GetAll
{
    public class GetAllApplicationUserQueryHandler : IRequestHandler<GetAllApplicationUserQuery, ResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
        private IApplicationUserService _applicationUserService;

        public GetAllApplicationUserQueryHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<ResponseDto<IEnumerable<ApplicationUserResponse>>> Handle(GetAllApplicationUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.GetAllAsync(request);
            return result;
        }
    }
}
