using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Dtos;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.ApplicationRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Create;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Delete;
using AuthenticationService.Application.Features.ApplicationRole.Commands.Update;
using AuthenticationService.Application.Features.ApplicationRole.Commands.AssignMenusToRole;
using AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole;
using AuthenticationService.Application.Features.ApplicationRole.Queries.Get;
using AuthenticationService.Application.Features.ApplicationRole.Queries.GetAll;
using AuthenticationService.Application.Features.ApplicationRole.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationRole.Queries.Search;
using AuthenticationService.Application.Features.ApplicationRole.Queries.SearchPaginated;
using AuthenticationService.Shared.Resources;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthenticationService.API.Controllers
{
    // Only authentication is required; no specific authorization policies or roles are applied.
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public class ApplicationRoleController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationRoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateApplicationRoleCommand request)
        {
            var responseDto = (ResponseDto<ApplicationRoleResponse>?) await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationRoleResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationRoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] UpdateApplicationRoleCommand request)
        {
            request.Id = id;
            var responseDto = (ResponseDto<ApplicationRoleResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationRoleResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            var request = new DeleteApplicationRoleCommand() {  Id = id };
            var responseDto = (ResponseDto<object>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<object>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("{roleId}/menus")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationRoleMenuResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Assign menus to a role",
            Description = "**Assigns a set of menus to a specific role**. If the menus have already been assigned to the role, " +
                          "the operation does nothing, making it **idempotent**. " +
                          "Subsequent requests with the same role and menus will have no effect, preventing duplicate assignments."
        )]
        public async Task<ActionResult> AssignMenusToRole([FromRoute] string roleId, [FromBody] AssignMenusToRoleCommand request)
        {
            request.ApplicationRoleId = roleId;
            var responseDto = (ResponseDto<ApplicationRoleMenuResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationRoleMenuResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpDelete("{roleId}/menus")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationRoleMenuResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Remove menus from a role",
            Description = "**Removes a set of menus from a specific role**. If the menus have already been removed from the role, " +
                          "the operation does nothing, making it **idempotent**. " +
                          "Subsequent requests with the same role and menus will have no effect."
        )]
        public async Task<ActionResult> RemoveMenusFromRole([FromRoute] string roleId, [FromBody] RemoveMenusFromRoleCommand request)
        {
            request.ApplicationRoleId = roleId;
            var responseDto = (ResponseDto<ApplicationRoleMenuResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationRoleMenuResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationRoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([FromRoute] string id)
        {
            var request = new GetApplicationRoleQuery() { Id = id };
            var responseDto = (ResponseDto<ApplicationRoleResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationRoleResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ApplicationRoleResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var request = new GetAllApplicationRoleQuery();
            var responseDto = (ResponseDto<IEnumerable<ApplicationRoleResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ApplicationRoleResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("paginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllPaginated([FromBody] GetAllPaginatedApplicationRoleQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ApplicationRoleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search([FromBody] SearchApplicationRoleQuery request)
        {
            var responseDto = (ResponseDto<IEnumerable<ApplicationRoleResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ApplicationRoleResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("searchpaginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchPaginated([FromBody] SearchPaginatedApplicationRoleQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ApplicationRoleResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }
    }
}
