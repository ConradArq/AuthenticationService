using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.GetAll
{
    public class GetAllApplicationUserQuery : RequestDto, IRequest<ResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
    }
}
