using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ListUsers([FromQuery] UserQueryParam userQueryParam)
        {
            try
            {
                return Ok(await _getAllUseCase.Execute(userQueryParam).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("users")]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        public IActionResult Create([FromBody] AdminCreateUserRequest userCreateRequest)
        {
            if (!userCreateRequest.IsValid())
                return BadRequest("Invalid details provided");

            try
            {
                return Created("Created", _createUserRequestUseCase.AdminExecute(userCreateRequest));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("users/{userId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult UpdateUser([FromRoute] int userId, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            try
            {
                return Ok(_updateUserRequestUseCase.Execute(userId, userUpdateRequest));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("users/{userId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            try
            {
                if (_deleteUserRequestUseCase.Execute(userId))
                    return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            return BadRequest("Could not delete the user");
        }
    }
}
