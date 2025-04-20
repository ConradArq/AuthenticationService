using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.Get
{
    public class GetApplicationMenuQuery : IRequest<ResponseDto<ApplicationMenuResponse>>
    {
        public int Id { get; set; }
    }
}
