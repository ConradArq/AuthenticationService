using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationUser.Commands.Update
{
    public class UpdateApplicationUserCommand : IRequest<ResponseDto<ApplicationUserResponse>>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        // Only used in OAuth flow, not in the standard registration process.
        [JsonIgnore]
        public bool IsRegistered { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public List<string>? RoleNames { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int? StatusId { get; set; }
    }
}
