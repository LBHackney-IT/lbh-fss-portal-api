using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticateUseCase _authenticateUseCase;
        private readonly ICreateUserRequestUseCase _createUserRequestUseCase;
        private readonly IConfirmUserUseCase _confirmUserUseCase;

        public AuthenticateController(IAuthenticateUseCase authenticateUseCase,
            ICreateUserRequestUseCase createUserRequestUseCase,
            IConfirmUserUseCase confirmUserUseCase)
        {
            _authenticateUseCase = authenticateUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
            _confirmUserUseCase = confirmUserUseCase;
        }

        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult CreateUser([FromQuery] UserCreateRequest userCreateRequest)
        {
            if (!userCreateRequest.IsValid())
                return BadRequest("Invalid details provided");
            try
            {
                var response = _createUserRequestUseCase.Execute(userCreateRequest);
                return Created("Created", response);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("registration/confirmation")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult ConfirmUser([FromQuery] ConfirmUserQueryParam userConfirmRequest)
        {
            ConfirmUserResponse response;

            //if (!userConfirmRequest.IsValid())
            //    return BadRequest("Invalid details provided");

            try
            {
                userConfirmRequest.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                response = _confirmUserUseCase.Execute(userConfirmRequest);
            }
            catch (UseCaseException e)
            {
                // Show a more detailed error message with call stack if running in development mode

                // TODO (MJC): Inject the environment variable below (DI)
                //if (_env.IsDev)
                //      return BadRequest(e.DeveloperErrorMessage);

                return BadRequest(e.ApiErrorMessage);
            }

            // Return the access token as a cookie, along with user metadata as JSON content
            Response.Cookies.Append(ConfirmUserResponse.AccessTokenName, response.AccessTokenValue);
            return Accepted(response.UserResponse);
        }

        [HttpPost]
        [Route("registration/confirmation/resend")]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ResendConfirmationCode([FromQuery] ConfirmationResendRequest confirmationResendRequest)
        {
            if (!confirmationResendRequest.IsValid())
                return BadRequest("Invalid details provided");
            try
            {
                _confirmUserUseCase.Resend(confirmationResendRequest);
                return Ok();
            }
            catch (Exception e)
            {
                throw;
            }
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
