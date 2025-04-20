using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Status.Queries.Get
{
    public class GetStatusQuery : IRequest<ResponseDto<ResponseStatus>>
    {
        public int Id { get; set; }
    }
}
