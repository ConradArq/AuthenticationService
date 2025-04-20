using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using AuthenticationService.Application.Strategies.Delete.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [BindNever]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the deletion mode for the user.
        /// Defaults to <see cref="DeletionMode.Hard"/> (i.e., a permanent deletion).
        /// Use <see cref="DeletionMode.Soft"/> to mark the user as deleted without removing data.
        /// </summary>
        [FromQuery]
        [DefaultValue(DeletionMode.Hard)]
        public DeletionMode DeletionMode { get; set; } = DeletionMode.Hard;
    }
}
