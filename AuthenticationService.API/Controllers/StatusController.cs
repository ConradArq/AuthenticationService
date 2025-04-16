using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Dtos;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.Status;
using AuthenticationService.Application.Features.Status.Commands.Create;
using AuthenticationService.Application.Features.Status.Commands.Delete;
using AuthenticationService.Application.Features.Status.Commands.Update;
using AuthenticationService.Application.Features.Status.Queries.Get;
using AuthenticationService.Application.Features.Status.Queries.GetAll;
using AuthenticationService.Application.Features.Status.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.Status.Queries.Search;
using AuthenticationService.Application.Features.Status.Queries.SearchPaginated;
using AuthenticationService.Shared.Resources;
using AuthenticationService.Application.Features.ApplicationUser.Queries.GetAll;

namespace AuthenticationService.API.Controllers
{
    // Only authentication is required; no specific authorization policies or roles are applied.
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public class StatusController : Controller
    {
        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponseDto<ResponseStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateStatusCommand request)
        {
            var responseDto = (ResponseDto<ResponseStatus>?) await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ResponseStatus>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<ResponseStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateStatusCommand request)
        {
            request.Id = id;
            var responseDto = (ResponseDto<ResponseStatus>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ResponseStatus>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteStatusCommand() {  Id = id };
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
        [ProducesResponseType(typeof(ApiResponseDto<ResponseStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var request = new GetStatusQuery() { Id = id };
            var responseDto = (ResponseDto<ResponseStatus>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ResponseStatus>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ResponseStatus>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll([FromQuery] GetAllStatusQuery? request)
        {
            var responseDto = (ResponseDto<IEnumerable<ResponseStatus>>?)await _mediator.Send(request!);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ResponseStatus>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("paginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ResponseStatus>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllPaginated([FromBody] GetAllPaginatedStatusQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ResponseStatus>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ResponseStatus>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ResponseStatus>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search([FromBody] SearchStatusQuery request)
        {
            var responseDto = (ResponseDto<IEnumerable<ResponseStatus>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ResponseStatus>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("searchpaginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ResponseStatus>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchPaginated([FromBody] SearchPaginatedStatusQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ResponseStatus>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ResponseStatus>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }
    }
}
