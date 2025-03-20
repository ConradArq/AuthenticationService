using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.Status.Queries.Search
{
    public class SearchStatusQuery : IRequest<ResponseDto<IEnumerable<ResponseStatus>>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int? StatusId { get; set; }
    }
}
