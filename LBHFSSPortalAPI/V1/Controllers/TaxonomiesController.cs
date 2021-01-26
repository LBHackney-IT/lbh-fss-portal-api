using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [Route("api/v1/taxonomies")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TaxonomiesController : BaseController
    {
        private ITaxonomyUseCase _taxonomyUseCase;

        public TaxonomiesController(ITaxonomyUseCase taxonomyUseCase)
        {
            _taxonomyUseCase = taxonomyUseCase;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(TaxonomyResponse), 201)]
        public IActionResult CreateTaxonomy(TaxonomyRequest taxonomyRequest)
        {
            //add validation
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    return BadRequest("no access_token cookie found in the request");
                var response = _taxonomyUseCase.ExecuteCreate(taxonomyRequest);
                if (response != null)
                    return Created(new Uri($"/{response.Id}", UriKind.Relative), response);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            // Validations
            return BadRequest(
            new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to create taxonomy." } });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(TaxonomyResponse), 200)]
        public IActionResult GetTaxonomy([FromRoute] int id)
        {
            //add validation
            var response = _taxonomyUseCase.ExecuteGetById(id);
            if (response != null)
                return Ok(response);

            // Validations
            return NotFound(
                new ErrorResponse($"Item not found") { Status = "Not found", Errors = new List<string> { $"Taxonomy with id {id} not found" } });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(TaxonomyResponseList), 200)]
        public IActionResult ListTaxonomies([FromQuery] int? vocabularyId)
        {
            var response = _taxonomyUseCase.ExecuteGet(vocabularyId);
            if (response != null)
                return Ok(response);

            return NotFound(
                new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to retrieve taxonomies." } });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{Id}")]
        [ProducesResponseType(typeof(TaxonomyResponse), 200)]
        public IActionResult PatchTaxonomy([FromRoute] int id, TaxonomyRequest taxonomyRequest)
        {
            try
            {
                var response = _taxonomyUseCase.ExecutePatch(id, taxonomyRequest);
                if (response != null)
                    return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return BadRequest(
                    new ErrorResponse($"Error updating taxonomy") { Status = "Bad request", Errors = new List<string> { $"An error occurred attempting to update taxonomy {id}: {e.Message}" } });
            }
            return BadRequest(
                new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to update taxonomy." } });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteTaxonomy([FromRoute] int id)
        {
            try
            {
                _taxonomyUseCase.ExecuteDelete(id);
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return BadRequest(
                    new ErrorResponse($"Error deleting taxonomy") { Status = "Bad request", Errors = new List<string> { $"An error occurred attempting to delete taxonomy {id}: {e.Message}" } });
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
            catch (ServiceTaxonomyExistsException ex)
            {
                var errorResponse = new ServiceErrorResponse
                {
                    ErrorMessage = ex.DevErrorMessage,
                    Services = ex.Services
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
