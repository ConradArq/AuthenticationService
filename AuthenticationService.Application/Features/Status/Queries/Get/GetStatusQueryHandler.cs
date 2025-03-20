using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Queries.Get
{
    public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, ResponseDto<ResponseStatus>>
    {
        private IStatusService _statusService;

        public GetStatusQueryHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<ResponseDto<ResponseStatus>> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _statusService.GetAsync(request.Id);
            return result;
        }
    }
}
