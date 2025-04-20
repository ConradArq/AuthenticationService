using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Dtos;
using AuthenticationService.Shared.Dtos;
using AuthenticationService.Application.Features.Authentication;
using AuthenticationService.Shared.Resources;
using AuthenticationService.Application.Features.Authentication.Commands.Login;
using AuthenticationService.Application.Features.Authentication.Commands.Logout;
using AuthenticationService.Application.Features.Authentication.Commands.TokenRefresh;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AuthenticationService.Application.Features.Authentication.Commands.GoogleLogin;
using AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin;
using AuthenticationService.Application.Features.Authentication.Commands.LoginVerifyOtp;

namespace AuthenticationService.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseDto<AuthenticationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Authenticate user and generate tokens",
            Description = "Authenticates the user using their credentials and **generates an access token and a refresh token**. " +
                          "The access token is used for authenticated requests, while the refresh token allows obtaining a new " +
                          "access token when the current one expires. " +
                          "This operation is **not idempotent** because each successful request generates a new refresh token and access token."
        )]
        public async Task<ActionResult> Login([FromBody] LoginCommand request)
        {
            var responseDto = (ResponseDto<AuthenticationResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<AuthenticationResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("login-verify-otp")]
        public async Task<IActionResult> LoginVerifyOtp([FromBody] LoginVerifyOtpCommand request)
        {
            var responseDto = (ResponseDto<AuthenticationResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<AuthenticationResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        /// <summary>
        /// Initiates the Google OAuth 2.0 login flow by redirecting the user to Google's authorization endpoint.
        /// 
        /// **Authentication Flow:**
        /// ASP.NET Core's authentication middleware handles the OAuth 2.0 authorization process when calling <see cref="ControllerBase.Challenge"/>:
        /// 1. Redirects the user to Google's OAuth authorization endpoint with query parameters such as client_id, redirect_uri, response_type...
        /// 2. Google prompts the user for login and consent and upon successful authentication returns an Authorization code.
        /// 3. Google redirects the user back to the **preconfigured callback URL** (see RedirectUrl in appsettings.json) appending 
        ///    the Authorization Code in the query string.
        /// 4. The middleware then **forwards the request to** <see cref="GoogleAuthCallback"/>        
        /// </summary>
        /// <returns>A challenge response that redirects the user to Google's OAuth 2.0 authentication page.</returns>
        [HttpGet("google-login")]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status302Found)]
        [SwaggerOperation(
            Summary = "Initiates Google login by redirecting the user to Google's OAuth 2.0 authorization endpoint",
            Description = "Redirects the user to Google for authentication. After successful login, Google redirects the user back to " +
                          "the application's configured callback URL."
        )]
        public IActionResult GoogleLogin()
        {
            // This is the action that Google's redirect URL will be mapped to after authentication.
            var redirectUrl = Url.Action(nameof(GoogleAuthCallback), "Authentication");

            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            // Redirects the user to Google for authentication.
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles the response from Google after the user logs in.
        /// 
        /// **Authentication Flow (continued from <see cref="GoogleLogin"/>):**
        /// 1. A call to <see cref="HttpContext.AuthenticateAsync"/> alows the authentication middleware for Google to extract the 
        /// Authorization Code from the internal store and **exchange the Authorization Code for tokens** 
        /// (Access Token, ID Token, and Refresh Token).
        /// 3. The middleware **extracts user claims from the ID token** and populates `<see cref="AuthenticateResult.Principal"/>`.
        ///    - The tokens returned from Google are stored in the <see cref="AuthenticateResult.Ticket"/> and must be retrieved manually if needed.
        ///    - To ensure tokens are included, configure Google authentication options properly.
        /// </summary>
        /// <returns>A JSON object containing a <see cref="ResponseDto{AuthenticationResponse}"/> with a newly generated JWT access token, 
        /// consistent with the authentication response used in the <see cref="Login"/> flow.</returns>
        [HttpGet("signin-google")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GoogleAuthCallback()
        {
            // Process the authentication response from Google.
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return Unauthorized();

            var request = new GoogleLoginCommand()
            {
                GoogleId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? authenticateResult.Principal.FindFirst("sub")?.Value ?? string.Empty,
                Email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                FirstName = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
                LastName = authenticateResult.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty
            };

            var responseDto = (ResponseDto<AuthenticationResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<AuthenticationResponse>.Ok(responseDto!);

            return Ok(apiResponseDto);        
        }

        [HttpGet("microsoft-login")]
        public IActionResult MicrosoftLogin()
        {
            var redirectUrl = Url.Action(nameof(MicrosoftAuthCallback), "Authentication");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Microsoft");
        }

        [HttpGet("signin-microsoft")]
        public async Task<IActionResult> MicrosoftAuthCallback()
        {
            // Process the authentication response from Microsoft.
            var authenticateResult = await HttpContext.AuthenticateAsync("Microsoft");

            if (!authenticateResult.Succeeded)
                return Unauthorized();

            // Extracts claims from the ID token and populate request
            var request = new MicrosoftLoginCommand()
            {
                MicrosoftId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? authenticateResult.Principal.FindFirst("sub")?.Value ?? string.Empty,
                Email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value
                        ?? authenticateResult.Principal.FindFirst("preferred_username")?.Value
                        ?? string.Empty,
                FirstName = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
                LastName = authenticateResult.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty
            };

            var responseDto = (ResponseDto<AuthenticationResponse>?)await _mediator.Send(request);
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<AuthenticationResponse>.Ok(responseDto!);

            return Ok(apiResponseDto);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(ApiResponseDto<AuthenticationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Invalidate the user's refresh token",
            Description = "**Revokes the logged-in user's refresh token**. This operation is **idempotent** because subsequent calls " +
                          "will have no additional effect once the refresh token has been invalidated. " +
                          "The access token is not revoked and remains valid until its expiration."
        )]
        public async Task<ActionResult> Logout()
        {
            var responseDto = (ResponseDto<object>?)await _mediator.Send(new LogoutCommand());
            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<object>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }

        [HttpPost("token/refresh")]
        [ProducesResponseType(typeof(ApiResponseDto<AuthenticationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto<object>), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Refreshes authentication tokens",
            Description = "**Generates a new access token and refresh token** from current refresh token. This operation is " +
                          "**statefully non-idempotent** because it not only generates new tokens but also revokes the previous refresh " +
                          "token permanently altering system state. It does not revoke access tokens which remain valid until expiration. " +
                          "Calling this endpoint multiple times with the same refresh token will cause subsequent requests to fail."
        )]
        public async Task<ActionResult> TokenRefresh([FromBody] TokenRefreshCommand request)
        {
            var responseDto = (ResponseDto<AuthenticationResponse>?)await _mediator.Send(request);

            if (responseDto == null)
            {
                throw new InvalidOperationException(GeneralMessages.MediatRErrorMessage);
            }

            var apiResponseDto = ApiResponseDto<AuthenticationResponse>.Ok(responseDto!);
            return Ok(apiResponseDto);
        }
    }
}
