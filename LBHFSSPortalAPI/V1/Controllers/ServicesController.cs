using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServicesController : BaseController
    {
        private ICreateServiceUseCase _createServiceUseCase;
        private IGetServicesUseCase _getServicesUseCase;

        public ServicesController(ICreateServiceUseCase createServiceUseCase, IGetServicesUseCase getServicesUseCase)
        {
            _createServiceUseCase = createServiceUseCase;
            _getServicesUseCase = getServicesUseCase;
        }

        [Route("services/{serviceId}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetService([FromRoute] int serviceId)
        {
            try
            {
                var response = await _getServicesUseCase.Execute(serviceId).ConfigureAwait(false);
                return Created("Created", response);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("services}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetService([FromQuery] ServicesQueryParam queryParam)
        {
            try
            {
                return Ok(await _getServicesUseCase.Execute(queryParam).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("services")]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddService([FromBody] AddServiceRequest request)
        {
            try
            {
                var response = await _createServiceUseCase.Execute(request).ConfigureAwait(false);
                return Created("Created", response);
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }
    }
}
