using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;

namespace AuthenticationService.Application.Features.ApplicationRole.Queries.Search
{
    public class SearchApplicationRoleQuery : RequestDto, IRequest<ResponseDto<IEnumerable<ApplicationRoleResponse>>>
    {
        public string? Name { get; set; }
    }
}
