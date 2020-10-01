using System;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Controllers;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class OrganizationsControllerTests
    {
        private OrganisationsController _classUnderTest;
        private Mock<IOrganisationsUseCase> _mockUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockUseCase = new Mock<IOrganisationsUseCase>();
            _classUnderTest = new OrganisationsController(_mockUseCase.Object);
        }

        #region Create Organisation

        [TestCase(TestName = "When the organisations controller CreateOrganisation action is called the OrganisationsUseCase ExecuteCreate method is called once with data provided")]
        public void CreateOrganisationControllerActionCallsTheOrganisationsUseCase()
        {
            var requestParams = Randomm.Create<OrganisationRequest>();
            _classUnderTest.CreateOrganisation(requestParams);
            _mockUseCase.Verify(uc => uc.ExecuteCreate(It.Is<OrganisationRequest>(p => p == requestParams)), Times.Once);
        }

        [TestCase(TestName = "When the organisations controller CreateOrganisation action is called and the organisation gets created it returns a response with a status code")]
        public void ReturnsResponseWithStatus()
        {
            var expected = Randomm.Create<OrganisationResponse>();
            var reqParams = Randomm.Create<OrganisationRequest>();
            _mockUseCase.Setup(u => u.ExecuteCreate(It.IsAny<OrganisationRequest>())).Returns(expected);
            var response = _classUnderTest.CreateOrganisation(reqParams) as CreatedResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(201);
            response.Value.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region Get Organisation
        [TestCase(TestName = "When the organisations controller GetOrganisation action is called the OrganisationsUseCase ExecuteGet method is called once with id provided")]
        public void GetOrganisationControllerActionCallsTheOrganisationsUseCase()
        {
            var requestParam = Randomm.Create<int>();
            _classUnderTest.GetOrganisation(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteGet(It.Is<int>(p => p == requestParam)), Times.Once);
        }

        [TestCase(TestName = "When the organisations controller GetOrganisation action is called with an id, if matched an organisation with a 200 status code is returned")]
        public void ReturnsGetResponseWith200Status()
        {
            var expected = Randomm.Create<OrganisationResponse>();
            var requestParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteGet(It.Is<int>(p => p == requestParam))).Returns(expected);
            var response = _classUnderTest.GetOrganisation(requestParam) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase(TestName = "When the organisations controller GetOrganisation action is called with an id, if not matched an error response is returned with a 404 status code")]
        public void ReturnsGetResponseWith404Status()
        {
            var reqParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteGet(It.IsAny<int>())).Returns((OrganisationResponse) null);
            var response = _classUnderTest.GetOrganisation(reqParam) as NotFoundObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
        }

        #endregion

        #region Delete Organisation
        [TestCase(TestName = "When the organisations controller DeleteOrganisation action is called, the OrganisationsUseCase ExecuteDelete method is called once with id provided")]
        public void DeleteOrganisationControllerActionCallsTheOrganisationsUseCase()
        {
            var requestParam = Randomm.Create<int>();
            _classUnderTest.DeleteOrganisation(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteDelete(It.Is<int>(p => p == requestParam)), Times.Once);
        }

        [TestCase(TestName = "When the organisations controller DeleteOrganisation action is called with an id and the organisation gets deleted, a 200 status code is returned")]
        public void ReturnsEmptyResponseWith200Status()
        {
            var requestParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.Is<int>(p => p == requestParam)));
            var response = _classUnderTest.DeleteOrganisation(requestParam) as OkResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
        }

        [TestCase(TestName = "When the organisations controller DeleteOrganisation action is called with an id, if not matched an error response, it is returned with a 400 status code")]
        public void ReturnsEmptyResponseWith400Status()
        {
            var reqParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.IsAny<int>())).Throws<InvalidOperationException>();
            var response = _classUnderTest.DeleteOrganisation(reqParam) as BadRequestObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
        }
        #endregion

        #region Patch Organisation
        [TestCase(TestName = "When the organisations controller PatchOrganisation action is called, the OrganisationsUseCase ExecutePatch method is called once with data and id provided")]
        public void PatchOrganisationControllerActionCallsTheOrganisationsUseCase()
        {
            var requestParams = Randomm.Create<OrganisationRequest>();
            var requestId = Randomm.Id();
            _classUnderTest.PatchOrganisation(requestId, requestParams);
            _mockUseCase.Verify(uc => uc.ExecutePatch(It.Is<int>(p => p == requestId), It.Is<OrganisationRequest>(p => p == requestParams)), Times.Once);
        }

        [TestCase(TestName = "When the organisations controller PatchOrganisation action is called and the organisation gets patched it returns a response with a status code")]
        public void PatchReturnsResponseWithStatus()
        {
            var expected = Randomm.Create<OrganisationResponse>();
            var id = Randomm.Id();
            var reqParams = Randomm.Create<OrganisationRequest>();
            _mockUseCase.Setup(u => u.ExecutePatch(It.IsAny<int>(), It.IsAny<OrganisationRequest>())).Returns(expected);
            var response = _classUnderTest.PatchOrganisation(id, reqParams) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}
