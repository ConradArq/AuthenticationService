using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.Status;
using AuthenticationService.Application.Features.Status.Commands.Create;
using AuthenticationService.Application.Features.Status.Commands.Update;
using AuthenticationService.Application.Features.Status.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.Status.Queries.Search;
using AuthenticationService.Application.Features.Status.Queries.SearchPaginated;

namespace AuthenticationService.Application.Interfaces.Services
{
    public interface IStatusService
    {
        Task<ResponseDto<ResponseStatus>> CreateAsync(CreateStatusCommand request);
        Task<ResponseDto<ResponseStatus>> UpdateAsync(int id, UpdateStatusCommand request);
        Task<ResponseDto<object>> DeleteAsync(int id);
        Task<ResponseDto<ResponseStatus>> GetAsync(int id);
        Task<ResponseDto<IEnumerable<ResponseStatus>>> GetAllAsync(RequestDto? requestDto);
        Task<PaginatedResponseDto<IEnumerable<ResponseStatus>>> GetAllPaginatedAsync(GetAllPaginatedStatusQuery request);
        Task<ResponseDto<IEnumerable<ResponseStatus>>> SearchAsync(SearchStatusQuery request);
        Task<PaginatedResponseDto<IEnumerable<ResponseStatus>>> SearchPaginatedAsync(SearchPaginatedStatusQuery request);
    }
}