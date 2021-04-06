using System;
using System.Security.Claims;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Controllers;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class ServicesControllerTests
    {
        private ServicesController _classUnderTest;
        private Mock<ICreateServiceUseCase> _mockCreateServiceUseCase;
        private Mock<IGetServicesUseCase> _mockGetServicesUseCase;
        private Mock<IUpdateServiceUseCase> _mockUpdateServiceUseCase;
        private Mock<IDeleteServiceUseCase> _mockDeleteServiceUseCase;
        private Mock<IGetAddressesUseCase> _mockGetAddressesUseCase;
        private Mock<IServiceImageUseCase> _mockServiceImageUseCase;
        public ServicesControllerTests()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new Claim(ClaimTypes.NameIdentifier, "1"), new Claim(ClaimTypes.Role, "Admin"), }, "mock"));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = $"access_token=xxxxx";
            httpContext.User = user;

            _mockServiceImageUseCase = new Mock<IServiceImageUseCase>();
            _mockGetAddressesUseCase = new Mock<IGetAddressesUseCase>();
            _mockDeleteServiceUseCase = new Mock<IDeleteServiceUseCase>();
            _mockUpdateServiceUseCase = new Mock<IUpdateServiceUseCase>();
            _mockGetServicesUseCase = new Mock<IGetServicesUseCase>();
            _mockCreateServiceUseCase = new Mock<ICreateServiceUseCase>();
            _classUnderTest = new ServicesController(
                _mockCreateServiceUseCase.Object,
                _mockGetServicesUseCase.Object,
                _mockDeleteServiceUseCase.Object,
                _mockUpdateServiceUseCase.Object,
                _mockGetAddressesUseCase.Object,
                _mockServiceImageUseCase.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        [TestCase(TestName =
            "When the services controller DeleteImage action is called with valid parameters the service image delete method gets called")]
        public void CallsImageDeleteMethodWhenImageDeleteIsCalled()
        {
            var serviceId = Randomm.Id();
            var imageId = Randomm.Id();
            _classUnderTest.DeleteServiceImage(serviceId, imageId);
            _mockServiceImageUseCase.Verify(siuc => siuc.ExecuteDelete(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName =
            "When the services controller DeleteImage action is called without a service id a bad request result is returned")]
        public void ReturnsBadRequestWhenImageDeleteIsCalledWithNoServiceId()
        {
            var imageId = Randomm.Id();
            var response = _classUnderTest.DeleteServiceImage(null, imageId).Result as BadRequestObjectResult;
            response.StatusCode.Should().Be(400);
        }

        [TestCase(TestName =
            "When the services controller DeleteImage action is called without an image id a bad request result is returned")]
        public void ReturnsBadRequestWhenImageDeleteIsCalledWithNoImageId()
        {
            var serviceId = Randomm.Id();
            var response = _classUnderTest.DeleteServiceImage(serviceId, null).Result as BadRequestObjectResult;
            response.StatusCode.Should().Be(400);
        }

        [TestCase(TestName =
                "When the services controller DeleteImage action is called with an id and the service gets deleted, a 200 status code is returned")]
        public void ReturnsEmptyResponseWith200StatusIfImageIsDeleted()
        {
            var serviceId = Randomm.Id();
            var imageId = Randomm.Id();
            var response = _classUnderTest.DeleteServiceImage(serviceId, imageId).Result as OkResult;
            response.StatusCode.Should().Be(200);
        }
    }
}
