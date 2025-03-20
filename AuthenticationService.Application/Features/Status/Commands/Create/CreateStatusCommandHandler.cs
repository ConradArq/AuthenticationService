using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Commands.Create
{
    public class CreateStatusCommandHandler : IRequestHandler<CreateStatusCommand, ResponseDto<ResponseStatus>>
    {
        private IStatusService _statusService;

        public CreateStatusCommandHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<ResponseDto<ResponseStatus>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _statusService.CreateAsync(request);
            return result;
        }
    }
}
