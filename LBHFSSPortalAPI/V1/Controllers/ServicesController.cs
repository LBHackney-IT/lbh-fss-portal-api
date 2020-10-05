using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServicesController : BaseController
    {
        private ICreateServiceUseCase _createServiceUseCase;
        private IGetServicesUseCase _getServicesUseCase;
        private readonly IUpdateServiceUseCase _updateServiceUseCase;
        private readonly IDeleteServiceUseCase _deleteServiceUseCase;

        public ServicesController(ICreateServiceUseCase createServiceUseCase,
                                  IGetServicesUseCase getServicesUseCase,
                                  IDeleteServiceUseCase deleteServiceUseCase,
                                  IUpdateServiceUseCase updateServiceUseCase)
        {
            _createServiceUseCase = createServiceUseCase;
            _getServicesUseCase = getServicesUseCase;
            _deleteServiceUseCase = deleteServiceUseCase;
            _updateServiceUseCase = updateServiceUseCase;
        }

        [Route("services/{serviceId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetService([FromRoute] int serviceId)
        {
            try
            {
                return Ok(await _getServicesUseCase.Execute(serviceId).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("services")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServices([FromQuery] ServicesQueryParam queryParam)
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
        public async Task<IActionResult> AddService([FromBody] ServiceRequest request)
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

        [Route("services/{serviceId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateService([FromBody] ServiceRequest request, [FromRoute] int serviceId)
        {
            try
            {
                return Ok(await _updateServiceUseCase.Execute(request, serviceId).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("services/{serviceId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteService([FromRoute] int serviceId)
        {
            try
            {
                await _deleteServiceUseCase.Execute(serviceId).ConfigureAwait(false);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Route("address-lookup")]
        [HttpGet]
        [ProducesResponseType(typeof(List<AddressLookupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LookupAddress([FromQuery] string postcode)
        {
            try
            {
                var response = new AddressLookupResponse()
                {
                    Longitude = 999,
                    Latitude = 999,
                    Uprn = 999,
                    Address1 = "NOT_YET_IMPLEMENTED_STUBBED_DATA",
                    Address2 = "NOT_YET_IMPLEMENTED_STUBBED_DATA",
                    City = "NOT_YET_IMPLEMENTED_STUBBED_DATA",
                    StateProvince = "NOT_YET_IMPLEMENTED_STUBBED_DATA",
                    PostalCode = postcode ?? "NOT_YET_IMPLEMENTED_STUBBED_DATA",
                    Country = "NOT_YET_IMPLEMENTED_STUBBED_DATA"
                };

                return Ok(response);
                //return Ok(await _getServicesUseCase.LookupAddress(queryParam).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

    }
}
