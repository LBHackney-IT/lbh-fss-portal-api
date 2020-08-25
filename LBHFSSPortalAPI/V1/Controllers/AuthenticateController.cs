using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Validations;

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
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status201Created)]
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

        [HttpPost]
        [Route("registration/confirmation")]
        [ProducesResponseType(typeof(UsersResponseList), StatusCodes.Status200OK)]
        public IActionResult ConfirmUser([FromBody] UserConfirmRequest userConfirmRequest)
        {
            if (!userConfirmRequest.IsValid())
                return BadRequest("Invalid details provided");
            try
            {
                var response = _confirmUserUseCase.Execute(userConfirmRequest);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("session")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public IActionResult LoginUser([FromQuery] LoginUserQueryParam loginUserQueryParam)
        {
            return Ok(_authenticateUseCase.Execute(loginUserQueryParam));
        }

    }
}
