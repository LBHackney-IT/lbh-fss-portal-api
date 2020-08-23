using System;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private IGetAllUsersUseCase _getAllUseCase;
        private ICreateUserRequestUseCase _createUserRequestUseCase;
        private IConfirmUserRegUseCase _confirmUserRegUseCase;

        public UsersController(IGetAllUsersUseCase getAllUseCase,
                               ICreateUserRequestUseCase createUserRequestUseCase,
                               IConfirmUserRegUseCase confirmUserRegUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
            _confirmUserRegUseCase = confirmUserRegUseCase;
        }

        [Route("api/v1/users")]
        [HttpGet]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ListUsers([FromQuery] UserQueryParam userQueryParam)
        {
            return Ok(_getAllUseCase.Execute(userQueryParam));
        }

        [Route("api/v1/registration")]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult CreateUser([FromBody] UserCreateRequest userCreateRequest)
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

        /// <summary>
        /// Attempts to confirm the users registration (validate verification code)
        /// </summary>
        [Route("api/v1/registration/confirmation")]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult ConfirmUserRegistration([FromQuery] ConfirmUserQueryParam confirmUserQueryParam)
        {
            ConfirmUserResponse response;

            try
            {
                confirmUserQueryParam.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                response = _confirmUserRegUseCase.Execute(confirmUserQueryParam);
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
            return Accepted(response);
        }
    }
}
