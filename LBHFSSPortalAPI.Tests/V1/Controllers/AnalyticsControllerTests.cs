using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Controllers;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class AnalyticsControllerTests
    {
        private AnalyticsController _classUnderTest;
        private Mock<IAnalyticsUseCase> _mockUseCase;

        public AnalyticsControllerTests()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = $"access_token=xxxxx";
            _mockUseCase = new Mock<IAnalyticsUseCase>();
            _classUnderTest = new AnalyticsController(_mockUseCase.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        #region GetAnalytics

        [TestCase(TestName = "When the analytics controller Get analytics action is called with a request the AnalyticsUseCase ExecuteGet method is called once")]
        public void GetAnalyticsControllerActionCallsTheAnalyticsUseCaseWhenRequestProvided()
        {
            var requestParam = Randomm.Create<AnalyticsRequest>();
            _classUnderTest.GetAnalytics(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteGet(It.Is<AnalyticsRequest>(p => p == requestParam)), Times.Once);
        }

        [TestCase(TestName = "When the analytics controller Get analytics action is called with a request a 200 status code is returned")]
        public void ReturnsListResponseWith200Status()
        {
            var expected = new AnalyticsResponseList()
            {
                AnalyticEvents = Randomm.CreateMany<AnalyticsResponse>().ToList()
            };
            var requestParam = Randomm.Create<AnalyticsRequest>();
            _mockUseCase.Setup(u => u.ExecuteGet(It.Is<AnalyticsRequest>(p => p == requestParam))).Returns(expected);
            var response = _classUnderTest.GetAnalytics(requestParam) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }
        #endregion

    }
}
