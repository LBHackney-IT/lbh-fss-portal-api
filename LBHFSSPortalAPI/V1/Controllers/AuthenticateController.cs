using LBHFSSPortalAPI.V1.Boundary;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IGetUserUseCase _getUserRequestUseCase;

        public AuthenticateController(IAuthenticateUseCase authenticateUseCase,
            ICreateUserRequestUseCase createUserRequestUseCase,
            IConfirmUserUseCase confirmUserUseCase,
            IGetUserUseCase getUserRequestUseCase)
        {
            _authenticateUseCase = authenticateUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
            _confirmUserUseCase = confirmUserUseCase;
            _getUserRequestUseCase = getUserRequestUseCase;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        public IActionResult CreateUser([FromBody] UserCreateRequest userCreateRequest)
        {
            if (!userCreateRequest.IsValid())
                return BadRequest("Invalid details provided");
            try
            {
                var response = _createUserRequestUseCase.Execute(userCreateRequest);
                // TODO: Return user URI instead of 'Created'
                return Created("Created", response);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration/confirmation")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult ConfirmUser([FromBody] UserConfirmRequest userConfirmRequest)
        {
            UserResponse response;

            if (!userConfirmRequest.IsValid())
                return BadRequest("Invalid details provided");

            try
            {
                userConfirmRequest.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                response = _confirmUserUseCase.Execute(userConfirmRequest);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            return Accepted(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("invitation/confirmation")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        public IActionResult ConfirmInvitation([FromBody] ResetPasswordQueryParams userConfirmRequest)
        {
            LoginUserResponse response;

            try
            {
                response = _authenticateUseCase.ExecuteFirstLogin(userConfirmRequest, HttpContext.Connection.RemoteIpAddress.ToString());
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            return Accepted(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration/confirmation/resend-request")]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ResendConfirmationCode([FromBody] ConfirmationResendRequest confirmationResendRequest)
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
        [AllowAnonymous]
        [Route("session")]
        [HttpPost]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        public IActionResult LoginUser([FromBody] LoginUserQueryParam queryParam)
        {
            try
            {
                queryParam.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                var response = _authenticateUseCase.ExecuteLoginUser(queryParam);
                Response.Cookies.Append(Cookies.AccessTokenName, response.AccessToken);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [Route("account")]
        [HttpGet]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult GetLoggedInUser()
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    return BadRequest("no access_token cookie found in the request");

                return Ok(_getUserRequestUseCase.Execute(AccessToken));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Logs the user out of the API removing session information
        /// </summary>
        [Authorize(Roles = "VCSO, Admin, Viewer")]
        [Route("logout")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LogoutUser()
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    return BadRequest("no access_token cookie found in the request");

                _authenticateUseCase.ExecuteLogoutUser(AccessToken);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            return Ok();
        }
    }
}
