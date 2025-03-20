using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using AuthenticationService.Application.Features.ApplicationUser;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.Identity.Commands.Register
{
    public class RegisterCommand: IRequest<ResponseDto<ApplicationUserResponse>>
    {
        // Only used in OAuth flow, not in the standard registration process.
        [JsonIgnore]
        public string? GoogleId { get; set; }
        // Only used in OAuth flow, not in the standard registration process.
        [JsonIgnore]
        public string? MicrosoftId { get; set; }
        // Only used in OAuth flow, not in the standard registration process.
        // IsRegistered is true in all cases except when registering a new user through the OAuth flow.
        [JsonIgnore]
        public bool IsRegistered { get; set; } = true;
        // Only used in OAuth flow, not in the standard registration process.
        // EmailConfirmed is false in all cases except when registering a new user through the OAuth flow.
        [JsonIgnore]
        public bool EmailConfirmed { get; set; } = false;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Password { get; set; } = string.Empty;

        public List<string> RoleNames { get; set; } = new List<string>();

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int StatusId { get; set; } = (int)Domain.Enums.Status.Active;
    }
}
