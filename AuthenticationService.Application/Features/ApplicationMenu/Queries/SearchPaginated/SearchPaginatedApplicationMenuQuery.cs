using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationMenu.Queries.SearchPaginated
{
    public class SearchPaginatedApplicationMenuQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>>
    {
        public string? Title { get; set; } = string.Empty;
        public string? Path { get; set; }
        public string? IconType { get; set; }
        public string? Icon { get; set; }
        public string? Class { get; set; }
        public bool? GroupTitle { get; set; }
        public string? Badge { get; set; }
        public string? BadgeClass { get; set; }
        public int? Order { get; set; }

        public int? ParentApplicationMenuId { get; set; }

        public string? roleId { get; set; }
    }
}
