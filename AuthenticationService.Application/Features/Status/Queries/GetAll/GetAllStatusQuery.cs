using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.Status.Queries.GetAll
{
    public class GetAllStatusQuery : IRequest<ResponseDto<IEnumerable<ResponseStatus>>>
    {
    }
}
