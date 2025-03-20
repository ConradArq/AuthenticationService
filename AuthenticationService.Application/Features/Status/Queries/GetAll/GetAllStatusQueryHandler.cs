using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Queries.GetAll
{
    public class GetAllStatusQueryHandler : IRequestHandler<GetAllStatusQuery, ResponseDto<IEnumerable<ResponseStatus>>>
    {
        private IStatusService _statusService;

        public GetAllStatusQueryHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<ResponseDto<IEnumerable<ResponseStatus>>> Handle(GetAllStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _statusService.GetAllAsync();
            return result;
        }
    }
}
