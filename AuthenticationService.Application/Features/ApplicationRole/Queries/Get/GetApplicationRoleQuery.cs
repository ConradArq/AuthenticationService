using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.Get
{
    public class GetApplicationRoleQuery : IRequest<ResponseDto<ApplicationRoleResponse>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
