using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.Get
{
    public class GetApplicationUserQuery : IRequest<ResponseDto<ApplicationUserResponse>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
