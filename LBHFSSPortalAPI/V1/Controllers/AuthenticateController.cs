using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticateUseCase _authenticateUseCase;

        public AuthenticateController(IAuthenticateUseCase authenticateUseCase)
        {
            _authenticateUseCase = authenticateUseCase;
        }

        /// <summary>
        /// Logs the user into API creating a new session
        /// </summary>
        [Route("api/v1/session")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LoginUser([FromQuery] LoginUserQueryParam loginUserQueryParam)
        {
            loginUserQueryParam.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            LoginUserResponse response;

            try
            {
                response = _authenticateUseCase.ExecuteLoginUser(loginUserQueryParam);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e.ApiErrorMessage);
            }

            Response.Cookies.Append(LoginUserResponse.AccessTokenName, response.AccessToken);
            return Ok();
        }

        /// <summary>
        /// Logs the user out of the API removing session information
        /// </summary>
        [Route("api/v1/logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LogoutUser([FromQuery] LogoutUserQueryParam queryParams)
        {
            try
            {
                _authenticateUseCase.ExecuteLogoutUser(queryParams);
            }
            catch (UseCaseException e)
            {
                // Show a more detailed error message with call stack if running in development mode

                // TODO (MJC): Inject the environment variable below (DI) and move up to common code in base class
                //if (_env.IsDev)
                //      return BadRequest(e.DeveloperErrorMessage);

                return BadRequest(e.ApiErrorMessage);
            }

            return Ok();
        }
    }
}
