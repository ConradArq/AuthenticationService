using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAll
{
    public class GetAllApplicationMenuQuery : IRequest<ResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
    }
}
