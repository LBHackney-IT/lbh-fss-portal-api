using System;
using System.Collections.Generic;
using System.IO;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
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
            new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to create organisation." } });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(OrganisationResponse), 200)]
        public IActionResult GetOrganisation([FromRoute] int id)
        {
            //add validation
            var response = _organisationsUseCase.ExecuteGet(id);
            if (response != null)
                return Ok(response);

            // Validations
            return NotFound(
                new ErrorResponse($"Item not found") { Status = "Not found", Errors = new List<string> { $"Organisation with id {id} not found" } });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(OrganisationResponseList), 200)]
        public IActionResult SearchOrganisations([FromQuery] OrganisationSearchRequest requestParams)
        {
            //add validation
            //try
            //{
            var response = _organisationsUseCase.ExecuteGet(requestParams).Result;
            return Ok(response);
            //}
            // catch (Exception e)
            // {
            //     Console.WriteLine(e.Message);
            //     return BadRequest(
            //         new ErrorResponse($"An error occurred") { Status = "Error", Errors = new List<string> { $"An error occurred while processing this request.  Please see logs for details." } });
            // }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{Id}")]
        [ProducesResponseType(typeof(OrganisationResponse), 200)]
        public IActionResult PatchOrganisation([FromRoute] int id, OrganisationRequest organisationRequest)
        {
            //add validation
            var response = _organisationsUseCase.ExecutePatch(id, organisationRequest);
            if (response != null)
                return Ok(response);

            // Validations
            return BadRequest(
                new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to create organisation." } });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteOrganisation([FromRoute] int id)
        {
            //add validation

            try
            {
                _organisationsUseCase.ExecuteDelete(id);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return BadRequest(
                    new ErrorResponse($"Item doesn't exist") { Status = "Bad request", Errors = new List<string> { $"An organisation with id {id} does not exist" } });
            }
        }
    }
}
