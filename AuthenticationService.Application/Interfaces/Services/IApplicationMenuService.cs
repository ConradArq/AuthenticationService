using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.ApplicationMenu;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Create;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Update;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.Search;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.SearchPaginated;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface IApplicationMenuService
    {
        Task<ResponseDto<ApplicationMenuResponse>> CreateAsync(CreateApplicationMenuCommand request);
        Task<ResponseDto<ApplicationMenuResponse>> UpdateAsync(int id, UpdateApplicationMenuCommand request);
        Task<ResponseDto<object>> DeleteAsync(int id);
        Task<ResponseDto<ApplicationMenuResponse>> GetAsync(int id);
        Task<ResponseDto<IEnumerable<ApplicationMenuResponse>>> GetAllAsync();
        Task<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>> GetAllPaginatedAsync(GetAllPaginatedApplicationMenuQuery request);
        Task<ResponseDto<IEnumerable<ApplicationMenuResponse>>> SearchAsync(SearchApplicationMenuQuery request);
        Task<PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>> SearchPaginatedAsync(SearchPaginatedApplicationMenuQuery request);
    }
}