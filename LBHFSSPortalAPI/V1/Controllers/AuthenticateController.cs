using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/authenticate")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticateUseCase _authenticateUseCase;

        public AuthenticateController(IAuthenticateUseCase authenticateUseCase)
        {
            _authenticateUseCase = authenticateUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public IActionResult LoginUser([FromQuery] LoginUserQueryParam loginUserQueryParam)
        {
            return Ok(_authenticateUseCase.Execute(loginUserQueryParam));
        }

    }
}
