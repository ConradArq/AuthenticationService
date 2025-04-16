using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.ApplicationUser;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Update;
using AuthenticationService.Application.Features.ApplicationUser.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationUser.Queries.Search;
using AuthenticationService.Application.Features.ApplicationUser.Queries.SearchPaginated;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Delete;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface IApplicationUserService
    {
        Task<ResponseDto<ApplicationUserResponse>> UpdateAsync(string id, UpdateApplicationUserCommand request);
        Task<ResponseDto<object>> DeleteAsync(DeleteApplicationUserCommand request);
        Task<ResponseDto<ApplicationUserResponse>> GetAsync(string id);
        Task<ResponseDto<IEnumerable<ApplicationUserResponse>>> GetAllAsync(RequestDto? requestDto);
        Task<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>> GetAllPaginatedAsync(GetAllPaginatedApplicationUserQuery request);
        Task<ResponseDto<IEnumerable<ApplicationUserResponse>>> SearchAsync(SearchApplicationUserQuery request);
        Task<PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>> SearchPaginatedAsync(SearchPaginatedApplicationUserQuery request);
    }
}