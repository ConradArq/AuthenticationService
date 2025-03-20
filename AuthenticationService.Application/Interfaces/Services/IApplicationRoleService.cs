using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.ApplicationRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Create;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Update;
using AuthenticationService.Application.Features.ApplicationRole.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationRole.Queries.Search;
using AuthenticationService.Application.Features.ApplicationRole.Queries.SearchPaginated;
using AuthenticationService.Application.Features.ApplicationRole.Commands.AssignMenusToRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface IApplicationRoleService
    {
        Task<ResponseDto<ApplicationRoleResponse>> CreateAsync(CreateApplicationRoleCommand request);
        Task<ResponseDto<ApplicationRoleResponse>> UpdateAsync(string id, UpdateApplicationRoleCommand request);
        Task<ResponseDto<object>> DeleteAsync(string id);
        Task<ResponseDto<ApplicationRoleMenuResponse>> AssignMenusToRoleAsync(AssignMenusToRoleCommand request);
        Task<ResponseDto<ApplicationRoleResponse>> GetAsync(string id);
        Task<ResponseDto<IEnumerable<ApplicationRoleResponse>>> GetAllAsync();
        Task<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>> GetAllPaginatedAsync(GetAllPaginatedApplicationRoleQuery request);
        Task<ResponseDto<IEnumerable<ApplicationRoleResponse>>> SearchAsync(SearchApplicationRoleQuery request);
        Task<PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>> SearchPaginatedAsync(SearchPaginatedApplicationRoleQuery request);
        Task<ResponseDto<ApplicationRoleMenuResponse>> RemoveMenusFromRoleAsync(RemoveMenusFromRoleCommand request);
    }
}