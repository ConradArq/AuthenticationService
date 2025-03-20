using MediatR;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Interfaces.Services;

namespace AuthenticationService.Application.Features.Status.Commands.Delete
{
    public class DeleteStatusCommandHandler : IRequestHandler<DeleteStatusCommand, ResponseDto<object>>
    {
        private IStatusService _statusService;

        public DeleteStatusCommandHandler(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public async Task<ResponseDto<object>> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            var result = await _statusService.DeleteAsync(request.Id);
            return result;
        }
    }
}
