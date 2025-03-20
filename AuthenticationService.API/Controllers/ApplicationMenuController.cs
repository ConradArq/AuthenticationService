using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Dtos;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.ApplicationMenu;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Create;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Delete;
using AuthenticationService.Application.Features.ApplicationMenu.Commands.Update;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.Get;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAll;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.GetAllPaginated;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.Search;
using AuthenticationService.Application.Features.ApplicationMenu.Queries.SearchPaginated;
using AuthenticationService.Shared.Resources;

namespace AuthenticationService.API.Controllers
{
    // Only authentication is required; no specific authorization policies or roles are applied.
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public class ApplicationMenuController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationMenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationMenuResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateApplicationMenuCommand request)
        {
            var responseDto = (ResponseDto<ApplicationMenuResponse>?) await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationMenuResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationMenuResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateApplicationMenuCommand request)
        {
            request.Id = id;
            var responseDto = (ResponseDto<ApplicationMenuResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationMenuResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "EntityOwnershipPolicy")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteApplicationMenuCommand() {  Id = id };
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
        [ProducesResponseType(typeof(ApiResponseDto<ApplicationMenuResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            var request = new GetApplicationMenuQuery() { Id = id };
            var responseDto = (ResponseDto<ApplicationMenuResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationMenuResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ApplicationMenuResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll()
        {
            var request = new GetAllApplicationMenuQuery();
            var responseDto = (ResponseDto<IEnumerable<ApplicationMenuResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ApplicationMenuResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("paginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllPaginated([FromBody] GetAllPaginatedApplicationMenuQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<ApplicationMenuResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search([FromBody] SearchApplicationMenuQuery request)
        {
            var responseDto = (ResponseDto<IEnumerable<ApplicationMenuResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<IEnumerable<ApplicationMenuResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("searchpaginated")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiPaginatedResponseDto<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SearchPaginated([FromBody] SearchPaginatedApplicationMenuQuery request)
        {
            var responseDto = (PaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiPaginatedResponseDto<IEnumerable<ApplicationMenuResponse>>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }
    }
}
