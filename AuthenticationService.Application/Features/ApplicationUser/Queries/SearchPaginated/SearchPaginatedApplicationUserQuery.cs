using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationUser.Queries.SearchPaginated
{
    public class SearchPaginatedApplicationUserQuery : PaginationRequestDto, IRequest<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<string>? RoleNames { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int? StatusId { get; set; }
    }
}
