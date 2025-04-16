using MediatR;
using AuthenticationService.Shared.Dtos;

namespace AuthenticationService.Application.Features.Status.Queries.Search
{
    public class SearchStatusQuery : RequestDto, IRequest<ResponseDto<IEnumerable<ResponseStatus>>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
