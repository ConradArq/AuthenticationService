using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest<ResponseDto<object>>
    {
    }
}
