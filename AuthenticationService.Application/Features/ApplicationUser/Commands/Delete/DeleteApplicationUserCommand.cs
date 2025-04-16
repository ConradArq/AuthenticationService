using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationUser.Commands.Delete
{
    /// <summary>
    /// Command to delete an application user.
    /// The user ID is expected from the route, and SoftDelete indicates whether to perform a soft or hard delete.
    /// </summary>
    public class DeleteApplicationUserCommand : IRequest<ResponseDto<object>>
    {
        /// <summary>
        /// The ID of the user to delete. This value is bound from the route.
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user should be soft-deleted.
        /// Defaults to false (i.e., perform a hard delete).
        /// </summary>
        public bool SoftDelete { get; set; } = false;
    }
}
