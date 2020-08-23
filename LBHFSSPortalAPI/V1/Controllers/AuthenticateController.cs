using LBHFSSPortalAPI.V1.Boundary.Requests;
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
            return Ok(_authenticateUseCase.ExecuteLoginUser(loginUserQueryParam));
        }


        /// <summary>
        /// Logs the user out of the API removing session information
        /// </summary>
        [Route("api/v1/logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LogoutUser([FromQuery] LogoutUserQueryParam logoutUserQueryParam)
        {
            try
            {
                _authenticateUseCase.ExecuteLogoutUser(logoutUserQueryParam);
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
