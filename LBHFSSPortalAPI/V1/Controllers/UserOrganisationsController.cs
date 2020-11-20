using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Handlers;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/user-links")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserOrganisationsController : BaseController
    {
        private IUserOrganisationLinksUseCase _userOrganisationLinksUseCase;

        public UserOrganisationsController(IUserOrganisationLinksUseCase userOrganisationLinksUseCase)
        {
            _userOrganisationLinksUseCase = userOrganisationLinksUseCase;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(UserOrganisationLinkResponse), 201)]
        public IActionResult CreateUserOrganisation(UserOrganisationLinkRequest requestParams)
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    return BadRequest("no access_token cookie found in the request");
                var response = _userOrganisationLinksUseCase.ExecuteCreate(requestParams);
                if (response != null)
                    return Created(new Uri($"/{response.Id}", UriKind.Relative), response);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            return BadRequest(
            new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to create organisation." } });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{userId}")]
        public IActionResult DeleteUserOrganisation([FromRoute] int userId)
        {
            try
            {
                _userOrganisationLinksUseCase.ExecuteDelete(userId);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return BadRequest(
                    new ErrorResponse($"Error deleting organisation") { Status = "Bad request", Errors = new List<string> { $"An error occurred attempting to delete user organisation with userId {userId}: {e.Message}" } });
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

    }
}
