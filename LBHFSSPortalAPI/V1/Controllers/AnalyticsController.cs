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
    [Route("api/v1/analytics-event")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AnalyticsController : BaseController
    {
        private IAnalyticsUseCase _analyticsUseCase;

        public AnalyticsController(IAnalyticsUseCase analyticsUseCase)
        {
            _analyticsUseCase = analyticsUseCase;
        }

        [Authorize(Roles = "Admin, VCSO")]
        [HttpGet]
        [ProducesResponseType(typeof(AnalyticsResponseList), 200)]
        public IActionResult GetAnalytics([FromQuery] AnalyticsRequest analyticsRequest)
        {
            var response = _analyticsUseCase.ExecuteGet(analyticsRequest);
            if (response != null)
                return Ok(response);

            return NotFound(
                new ErrorResponse($"Invalid request. ") { Status = "Bad request", Errors = new List<string> { "Unable to retrieve analytics." } });
        }
    }
}
