using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Dtos;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.ApplicationUser;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Delete;
using AuthenticationService.Application.Features.ApplicationUser.Commands.Update;
using AuthenticationService.Application.Features.ApplicationUser.Queries.Get;
using AuthenticationService.Application.Features.ApplicationUser.Queries.GetAll;
using AuthenticationService.Application.Features.ApplicationUser.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationUser.Queries.Search;
using AuthenticationService.Application.Features.ApplicationUser.Queries.SearchPaginated;
using AuthenticationService.Shared.Resources;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAll;

namespace AuthenticationService.API.Controllers
{
    // Only authentication is required; no specific authorization policies or roles are applied.
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public class ApplicationUserController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] UpdateApplicationUserCommand request)
        {
            request.Id = id;
            var responseDto = (ResponseDto<ApplicationUserResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationUserResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] string id, [FromQuery] bool softDelete = false)
        {
            var request = new DeleteApplicationUserCommand() {  Id = id, SoftDelete = softDelete };
            var responseDto = (ResponseDto<object>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<object>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([FromRoute] string id)
        {
            var request = new GetApplicationUserQuery() { Id = id };
            var responseDto = (ResponseDto<ApplicationUserResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationUserResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ApplicationUserResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll([FromQuery] GetAllApplicationUserQuery? request)
        {
            var responseDto = (ResponseDto<IEnumerable<ApplicationUserResponse>>?)await _mediator.Send(request!);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ApplicationUserResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("paginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ApplicationUserResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllPaginated([FromBody] GetAllPaginatedApplicationUserQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ApplicationUserResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ApplicationUserResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search([FromBody] SearchApplicationUserQuery request)
        {
            var responseDto = (ResponseDto<IEnumerable<ApplicationUserResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ApplicationUserResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("searchpaginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ApplicationUserResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchPaginated([FromBody] SearchPaginatedApplicationUserQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ApplicationUserResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ApplicationUserResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }
    }
}
