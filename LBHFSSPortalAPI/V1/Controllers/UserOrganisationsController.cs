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
        private IUserOrganisationUseCase _userOrganisationLinksUseCase;

        public UserOrganisationsController(IUserOrganisationUseCase userOrganisationLinksUseCase)
        {
            _userOrganisationLinksUseCase = userOrganisationLinksUseCase;
        }

        [Authorize(Roles = "Admin, VCSO")]
        [HttpPost]
        [ProducesResponseType(typeof(UserOrganisationResponse), 201)]
        public IActionResult CreateUserOrganisation(UserOrganisationRequest requestParams)
        {
            try
            {
                UserClaims userClaims = new UserClaims
                {
                    UserId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    UserRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value
                };
                LambdaLogger.Log($"UserID:{userClaims.UserId.ToString()} UserRole:{userClaims.UserRole}");
                if (string.IsNullOrEmpty(AccessToken))
                    return BadRequest("no access_token cookie found in the request");
                var response = _userOrganisationLinksUseCase.ExecuteCreate(requestParams, userClaims);
                if (response != null)
                    return Created(new Uri($"/{response.Id}", UriKind.Relative), response);
            }
            catch (InvalidOperationException e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return BadRequest(
                    new ErrorResponse($"Error creating userorganisation") { Status = "Bad request", Errors = new List<string> { $"An error occurred attempting to create userorganisation with userId {requestParams.UserId} and organisationId {requestParams.OrganisationId}: {e.Message}" } });
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
                    new ErrorResponse($"Error deleting userorganisation") { Status = "Bad request", Errors = new List<string> { $"An error occurred attempting to delete user organisation with userId {userId}: {e.Message}" } });
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

    }
}
