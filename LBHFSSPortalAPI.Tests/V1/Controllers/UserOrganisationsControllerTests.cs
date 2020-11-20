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
    public class UserOrganisationsControllerTests
    {
        private UserOrganisationsController _classUnderTest;
        private Mock<IUserOrganisationUseCase> _mockUseCase;

        [SetUp]
        public void SetUp()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = $"access_token={Randomm.Word()}";

            _mockUseCase = new Mock<IUserOrganisationUseCase>();
            _classUnderTest = new UserOrganisationsController(_mockUseCase.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        #region Create UserOrganisation
        [TestCase(TestName = "When the userorganisations controller CreateUserOrganisation action is called the UserOrganisationsUseCase ExecuteCreate method is called once with data provided")]
        public void CreateOrganisationControllerActionCallsTheOrganisationsUseCase()
        {
            var requestParams = Randomm.Create<UserOrganisationRequest>();
            _classUnderTest.CreateUserOrganisation(requestParams);
            _mockUseCase.Verify(uc => uc.ExecuteCreate(It.Is<UserOrganisationRequest>(p => p == requestParams)), Times.Once);
        }

        [TestCase(TestName = "When the userorganisations controller CreateUserOrganisation action is called and the UserOrganisationsUseCase gets created it returns a response with a status code")]
        public void ReturnsResponseWithStatus()
        {
            var expected = Randomm.Create<UserOrganisationResponse>();
            var requestParams = Randomm.Create<UserOrganisationRequest>();
            _mockUseCase.Setup(u => u.ExecuteCreate(It.IsAny<UserOrganisationRequest>())).Returns(expected);
            var response = _classUnderTest.CreateUserOrganisation(requestParams) as CreatedResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(201);
            response.Value.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region Delete Organisation
        [TestCase(TestName = "When the userorganisations controller DeleteUserOrganisation action is called, the UserOrganisationsUseCase ExecuteDelete method is called once with id provided")]
        public void DeleteUserOrganisationControllerActionCallsTheUserOrganisationsUseCase()
        {
            var requestParam = Randomm.Create<int>();
            _classUnderTest.DeleteUserOrganisation(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteDelete(requestParam), Times.Once);
        }

        [TestCase(TestName = "When the userorganisations controller DeleteUserOrganisation action is called with an id and the userorganisation gets deleted, a 200 status code is returned")]
        public void ReturnsEmptyResponseWith200Status()
        {
            var requestParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.Is<int>(p => p == requestParam)));
            var response = _classUnderTest.DeleteUserOrganisation(requestParam) as OkResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
        }
        [TestCase(TestName = "When the userorganisations controller DeleteUserOrganisation action is called with an id, if not matched an error response, it is returned with a 400 status code")]
        public void ReturnsEmptyResponseWith400Status()
        {
            UserClaims userClaims = new UserClaims
            {
            };
            var reqParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.IsAny<int>())).Throws<InvalidOperationException>();
            var response = _classUnderTest.DeleteUserOrganisation(reqParam) as BadRequestObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
        }
        #endregion
    }
}
