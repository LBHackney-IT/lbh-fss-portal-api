using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private IGetAllUsersUseCase _getAllUseCase;
        private ICreateUserRequestUseCase _createUserRequestUseCase;
        private IUpdateUserRequestUseCase _updateUserRequestUseCase;
        private IDeleteUserRequestUseCase _deleteUserRequestUseCase;

        public UsersController(IGetAllUsersUseCase getAllUseCase,
                               ICreateUserRequestUseCase createUserRequestUseCase,
                               IUpdateUserRequestUseCase updateUserRequestUseCase,
                               IDeleteUserRequestUseCase deleteUserRequestUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
            _updateUserRequestUseCase = updateUserRequestUseCase;
            _deleteUserRequestUseCase = deleteUserRequestUseCase;
        }

        [Route("users")]
        [HttpGet]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ListUsers([FromQuery] UserQueryParam userQueryParam)
        {
            return Ok(_getAllUseCase.Execute(userQueryParam));
        }

        [Route("users")]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        public IActionResult Create([FromBody] AdminCreateUserRequest userCreateRequest)
        {
            UserResponse response;

            if (!userCreateRequest.IsValid())
                return BadRequest("Invalid details provided");

            try
            {
                response = _createUserRequestUseCase.AdminExecute(userCreateRequest);
            }
            catch (UseCaseException e)
            {
                // Show a more detailed error message with call stack if running in development mode

                // TODO (MJC): Inject the environment variable below (DI)
                //if (_env.IsDev)
                //      return BadRequest(e.DeveloperErrorMessage);

                return BadRequest(e.UserErrorMessage);
            }

            return Created("Created", response);
        }

        [Route("users")]
        [HttpPatch]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult UpdateUser([FromQuery] int currentUserId, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            UserResponse response;

            try
            {
                response = _updateUserRequestUseCase.Execute(currentUserId, userUpdateRequest);
            }
            catch (UseCaseException e)
            {
                // Show a more detailed error message with call stack if running in development mode

                // TODO (MJC): Inject the environment variable below (DI)
                //if (_env.IsDev)
                //      return BadRequest(e.DeveloperErrorMessage);

                return BadRequest(e.UserErrorMessage);
            }

            return Ok(response);
        }

        [Route("users")]
        [HttpDelete]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult DeleteUser([FromQuery] int userId)
        {
            try
            {
                if (_deleteUserRequestUseCase.Execute(userId))
                    return Ok();
            }
            catch (UseCaseException e)
            {
                // Show a more detailed error message with call stack if running in development mode

                // TODO (MJC): Inject the environment variable below (DI)
                //if (_env.IsDev)
                //      return BadRequest(e.DeveloperErrorMessage);

                return BadRequest(e.UserErrorMessage);
            }

            return BadRequest("Could not delete the user");
        }
    }
}
