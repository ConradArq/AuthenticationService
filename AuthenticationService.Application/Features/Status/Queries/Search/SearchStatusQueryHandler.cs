using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Queries.Search
{
    public class SearchStatusQueryHandler : IRequestHandler<SearchStatusQuery, ResponseDto<IEnumerable<ResponseStatus>>>
    {
        private IStatusService _statusService;

        public SearchStatusQueryHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<ResponseDto<IEnumerable<ResponseStatus>>> Handle(SearchStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _statusService.SearchAsync(request);
            return result;
        }
    }
}
