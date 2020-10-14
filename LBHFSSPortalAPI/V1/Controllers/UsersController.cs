using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly IGetAllUsersUseCase _getAllUseCase;
        private readonly IGetUserUseCase _getUserRequestUseCase;
        private readonly ICreateUserRequestUseCase _createUserRequestUseCase;
        private readonly IUpdateUserRequestUseCase _updateUserRequestUseCase;
        private readonly IDeleteUserRequestUseCase _deleteUserRequestUseCase;
        private readonly IConfirmUserUseCase _confirmUserUseCase;

        public UsersController(IGetAllUsersUseCase getAllUseCase,
                               IGetUserUseCase getUserRequestUseCase,
                               ICreateUserRequestUseCase createUserRequestUseCase,
                               IUpdateUserRequestUseCase updateUserRequestUseCase,
                               IDeleteUserRequestUseCase deleteUserRequestUseCase,
                               IConfirmUserUseCase confirmUserUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _getUserRequestUseCase = getUserRequestUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
            _updateUserRequestUseCase = updateUserRequestUseCase;
            _deleteUserRequestUseCase = deleteUserRequestUseCase;
            _confirmUserUseCase = confirmUserUseCase;
        }

        [Authorize(Roles = "Viewer, Admin")]
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

        [Authorize(Roles = "Admin, VCSO, Viewer")]
        [Route("users/{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveUser([FromRoute] int userId)
        {
            try
            {
                return Ok(await _getUserRequestUseCase.Execute(userId).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin, VCSO, Viewer")]
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

        [Authorize(Roles = "Admin")]
        [Route("users/{userId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            try
            {
                _deleteUserRequestUseCase.Execute(userId);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("users/{userId}/resend")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ResendUserInvite([FromRoute] int userId)
        {
            try
            {
                _confirmUserUseCase.Resend(userId);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }
    }
}
