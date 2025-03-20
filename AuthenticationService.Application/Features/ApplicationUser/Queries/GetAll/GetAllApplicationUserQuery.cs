using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.GetAll
{
    public class GetAllApplicationUserQuery : IRequest<ResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
    }
}
