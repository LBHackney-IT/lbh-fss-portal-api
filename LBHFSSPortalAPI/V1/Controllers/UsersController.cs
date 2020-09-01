using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
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

        public UsersController(IGetAllUsersUseCase getAllUseCase, ICreateUserRequestUseCase createUserRequestUseCase)
        {
            _getAllUseCase = getAllUseCase;
            _createUserRequestUseCase = createUserRequestUseCase;
        }

        [Route("api/v1/users")]
        [HttpGet]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ListUsers([FromQuery] UserQueryParam userQueryParam)
        {
            return Ok(_getAllUseCase.Execute(userQueryParam));
        }

        [Route("api/v1/users")]
        [HttpPost]
        public IActionResult Create([FromBody] UserCreateRequest userCreateRequest)
        {
            _createUserRequestUseCase.AdminExecute(userCreateRequest);
            return Ok();
        }
    }
}
