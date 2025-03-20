using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.SearchPaginated
{
    public class SearchPaginatedApplicationRoleQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
        public string? Name { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int? StatusId { get; set; }
    }
}
