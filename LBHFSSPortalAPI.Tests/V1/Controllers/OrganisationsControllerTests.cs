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
    }
}
