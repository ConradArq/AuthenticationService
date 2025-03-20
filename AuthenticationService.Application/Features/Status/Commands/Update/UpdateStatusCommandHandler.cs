using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Commands.Update
{
    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, ResponseDto<ResponseStatus>>
    {
        private IStatusService _statusService;

        public UpdateStatusCommandHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<ResponseDto<ResponseStatus>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _statusService.UpdateAsync(request.Id, request);
            return result;
        }
    }
}
