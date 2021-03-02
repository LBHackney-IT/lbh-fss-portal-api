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
    [Route("api/v1/synonymGroups")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SynonymGroupsController : BaseController
    {
        private ISynonymsUseCase _synonymsUseCase;

        public SynonymGroupsController(ISynonymsUseCase synonymsUseCase)
        {
            _synonymsUseCase = synonymsUseCase;
        }


        /// <summary>
        /// POST will update all the synonyms groups and words within those groups if anything changes.
        /// </summary>
        /// <param name="googleFileId"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheetRange"></param>
        /// <param name="googleApiKey"></param>
        /// <remarks>
        /// Sample request:
        ///     POST /
        ///     {
        ///        "googleFileId": "xxxxxx"
        ///        "sheetName": "Synonyms"
        ///        "sheetRange": "A:AU"
        ///        "googleApiKey" : "xxxAAAyyy"
        ///     }
        ///     or
        ///     {
        ///        "googleFileId": null
        ///        "sheetName": null
        ///        "sheetRange": null
        ///        "googleApiKey" : null
        ///     }
        /// </remarks>
        //[Authorize(Roles = "Admin, VCSO")]
        [HttpPost]
        [Route("UpdateFromGoogleDrive")]
        [ProducesResponseType(typeof(SynonymsResponse), 200)]
        public IActionResult UpdateSynonyms([FromHeader(Name = "googleFileId")] string googleFileId,
            [FromHeader(Name = "sheetName")] string sheetName,
            [FromHeader(Name = "sheetRange")] string sheetRange,
            [FromHeader(Name = "googleApiKey")] string googleApiKey)
        {
            try
            {
                //if (string.IsNullOrEmpty(AccessToken))
                //    return BadRequest("no access_token cookie found in the request");
                if (string.IsNullOrEmpty(googleFileId))
                {
                    googleFileId = Environment.GetEnvironmentVariable("SYNONYMS_GOOGLE_FILE_ID") ??
                        "1V89AxVH-SXGloTmX6T2o-FG-any14qah-WOi3eaYelE";
                }
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheetName = Environment.GetEnvironmentVariable("SYNONYMS_SHEET_NAME") ??
                                "Synonyms";
                }
                if (string.IsNullOrEmpty(sheetRange))
                {
                    sheetRange = Environment.GetEnvironmentVariable("SYNONYMS_SHEET_RANGE") ??
                                 "A:AU";
                }
                if (string.IsNullOrEmpty(googleApiKey))
                {
                    googleApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                                   "LongApiGoogleKeyFromGoogleConsoleGoesHere";
                }

                SynonymUpdateRequest synonymsRequest = new SynonymUpdateRequest
                {
                    GoogleFileId = googleFileId,
                    SheetName = sheetName,
                    SheetRange = sheetRange,
                    GoogleApiKey = googleApiKey
                };
                var response = _synonymsUseCase.ExecuteUpdate(AccessToken, synonymsRequest);
                if (response != null)
                    return Ok(response);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }

            // Validations
            return BadRequest(
                new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to create organisation." } });
        }
    }
}