using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Dtos;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Resources;
using AuthenticationService.Application.Features.Identity;
using AuthenticationService.Application.Features.Identity.Commands.Register;
using AuthenticationService.Application.Features.Identity.Commands.ConfirmEmail;
using AuthenticationService.Application.Features.Identity.Commands.ChangePassword;
using AuthenticationService.Application.Features.Identity.Commands.ResetPassword;
using AuthenticationService.Application.Features.Identity.Commands.ConfirmResetPassword;
using AuthenticationService.Application.Features.ApplicationUser;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ApiResponseDto<ApplicationUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public class IdentityController : Controller
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            Summary = "Register a new user account",
            Description = "**Creates a new user account with the provided details**, including first name, last name, email, " +
                          "username (optional), and password. The user is assigned one or more roles upon registration. " +
                          "A confirmation email is sent to the user, requiring them to verify their email address before accessing the system. " +
                          "This operation is **not idempotent**, as each request creates a new user entry if the email is not already registered."
        )]
        public async Task<ActionResult> Register([FromBody] RegisterCommand request)
        {
            var responseDto = (ResponseDto<ApplicationUserResponse>?) await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationUserResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Confirm user email address",
            Description = "**Verifies a user's email address using a confirmation token**. " +
                          "This operation is initiated after registration, where the user receives an email with a confirmation link. " +
                          "Once confirmed, the user's email is marked as verified, allowing full access to the system. " +
                          "This operation is **idempotent in terms of state change**, meaning multiple calls with the same token have " +
                          "no additional effect on the user's email confirmation status. However, an email will be sent on the first call " +
                          "to confirm the email address."
        )]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
        {
            var responseDto = (ResponseDto<ApplicationUserResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<ApplicationUserResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Change user password",
            Description = "Allows an authenticated user to **update their password by providing the current password and a new password**. " +
                          "If the current password is incorrect or does not meet security requirements, the request fails. " +
                          "The user remains authenticated after changing the password, as ASP.NET Identity does not automatically " +
                          "invalidate tokens. However, if SecurityStamp validation is enabled, all existing authentication sessions " +
                          "will be invalidated, requiring the user to log in again." +
                          "This operation is **not idempotent**, as each request updates the user's password."
        )]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
        {
            var responseDto = (ResponseDto<object>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<object>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Request password reset",
            Description = "**Initiates the password reset process** by sending an email with a reset token. " +
                          "The user must provide their registered email address. " +
                          "A password reset link is generated and sent via email, allowing the user to create a new password. " +
                          "This operation is **not idempotent**, as each request generates a new reset token and sends an email to the user."
        )]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
        {
            var responseDto = (ResponseDto<object>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<object>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("confirm-reset-password")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Confirm password reset",
            Description = "**Completes the password reset process** by verifying the reset token and setting a new password. " +
                          "The user provides the reset token received via email, along with the new password. " +
                          "If the token is invalid or expired, the request fails. " +
                          "This operation is **not idempotent**, as token is invalidated after successful password reset."
        )]
        public async Task<ActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordCommand request)
        {
            var responseDto = (ResponseDto<object>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<object>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }      
    }
}
