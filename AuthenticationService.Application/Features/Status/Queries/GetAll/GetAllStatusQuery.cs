using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Status.Queries.GetAll
{
    public class GetAllStatusQuery : RequestDto, IRequest<ResponseDto<IEnumerable<ResponseStatus>>>
    {
    }
}
