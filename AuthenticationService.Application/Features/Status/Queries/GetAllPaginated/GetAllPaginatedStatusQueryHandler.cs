using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Queries.GetAllPaginated
{
    public class GetAllPaginatedStatusQueryHandler : IRequestHandler<GetAllPaginatedStatusQuery, PaginatedResponseDto<IEnumerable<ResponseStatus>>>
    {
        private IStatusService _statusService;

        public GetAllPaginatedStatusQueryHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<PaginatedResponseDto<IEnumerable<ResponseStatus>>> Handle(GetAllPaginatedStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _statusService.GetAllPaginatedAsync(request);
            return result;
        }
    }
}
