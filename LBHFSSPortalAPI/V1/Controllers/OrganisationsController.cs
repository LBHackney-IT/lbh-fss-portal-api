using System;
using System.Collections.Generic;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/organisations")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrganisationsController : BaseController
    {
        private IOrganisationsUseCase _organisationsUseCase;

        public OrganisationsController(IOrganisationsUseCase organisationsUseCase)
        {
            _organisationsUseCase = organisationsUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrganisationResponse), 201)]
        public IActionResult CreateOrganisation(OrganisationRequest organisationRequest)
        {
            //add validation
            var response = _organisationsUseCase.ExecuteCreate(organisationRequest);
            if (response != null)
                return Created(new Uri($"/{response.Id}", UriKind.Relative), response);

            // Validations
            return BadRequest(
            new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string>{"Unable to create organisation."} });
        }
    }
}
