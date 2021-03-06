using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Amazon.Lambda.Core;

namespace LBHFSSPortalAPI.V1.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ServicesController : BaseController
    {
        private readonly ICreateServiceUseCase _createServiceUseCase;
        private readonly IGetServicesUseCase _getServicesUseCase;
        private readonly IUpdateServiceUseCase _updateServiceUseCase;
        private readonly IDeleteServiceUseCase _deleteServiceUseCase;
        private readonly IGetAddressesUseCase _getAddressesUseCase;
        private readonly IServiceImageUseCase _serviceImageUseCase;

        public ServicesController(ICreateServiceUseCase createServiceUseCase,
                                  IGetServicesUseCase getServicesUseCase,
                                  IDeleteServiceUseCase deleteServiceUseCase,
                                  IUpdateServiceUseCase updateServiceUseCase,
                                  IGetAddressesUseCase getAddressesUseCase,
                                  IServiceImageUseCase serviceImageUseCase)

        {
            _createServiceUseCase = createServiceUseCase;
            _getServicesUseCase = getServicesUseCase;
            _deleteServiceUseCase = deleteServiceUseCase;
            _updateServiceUseCase = updateServiceUseCase;
            _getAddressesUseCase = getAddressesUseCase;
            _serviceImageUseCase = serviceImageUseCase;
        }

        [Authorize(Roles = "Admin, VCSO, Viewer")]
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

        [Authorize(Roles = "Viewer, VCSO, Admin")]
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

        [Authorize(Roles = "Admin, VCSO")]
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

        [Authorize(Roles = "Admin, VCSO")]
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

        [Authorize(Roles = "Admin, VCSO")]
        [Route("services/{serviceId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteService([FromRoute] int serviceId)
        {
            try
            {
                UserClaims userClaims = new UserClaims
                {
                    UserId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    UserRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value
                };
                LambdaLogger.Log($"UserID:{userClaims.UserId.ToString()} UserRole:{userClaims.UserRole}");

                await _deleteServiceUseCase.Execute(serviceId, userClaims).ConfigureAwait(false);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Authorize(Roles = "Admin, VCSO, Viewer")]
        [Route("address-lookup")]
        [HttpGet]
        [ProducesResponseType(typeof(List<AddressLookupResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LookupAddress([FromQuery] string postcode)
        {
            try
            {
                return Ok(await _getAddressesUseCase.Execute(postcode).ConfigureAwait(false));
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Authorize(Roles = "Admin, VCSO")]
        [Route("services/{serviceId}/image")]
        [HttpPost]
        [RequestSizeLimit(5000000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddServiceImage([FromForm] ServiceImageRequest request)
        {
            try
            {
                await _serviceImageUseCase.ExecuteCreate(request).ConfigureAwait(false);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }

        [Authorize(Roles = "Admin, VCSO")]
        [Route("services/{serviceId}/image/{imageId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteServiceImage([FromRoute(Name = "serviceId")] int? serviceId,
            [FromRoute(Name = "imageId")] int? imageId)
        {
            if (!(serviceId.HasValue && imageId.HasValue))
            {
                return BadRequest("Both service id and image id must be provided");
            }

            try
            {
                await _serviceImageUseCase.ExecuteDelete(serviceId.Value, imageId.Value).ConfigureAwait(false);
                return Ok();
            }
            catch (UseCaseException e)
            {
                return BadRequest(e);
            }
        }
    }
}
