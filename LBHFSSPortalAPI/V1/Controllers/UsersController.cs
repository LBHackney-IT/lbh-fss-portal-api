using System;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private IGetAllUsersUseCase _getAllUseCase;
        private ICreateUserRequestUseCase _createUserRequestUseCase;

        public UsersController(IGetAllUsersUseCase getAllUseCase, ICreateUserRequestUseCase createUserRequestUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ListUsers([FromQuery] UserQueryParam userQueryParam)
        {
            return Ok(_getAllUseCase.Execute(userQueryParam));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult CreateUser([FromBody] UserCreateRequest userCreateRequest)
        {
            if (!userCreateRequest.IsValid())
                return BadRequest("Invalid details provided");
            try
            {
                var response = _createUserRequestUseCase.Execute(userCreateRequest);
                return Created(string.Empty, null);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
