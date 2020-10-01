using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class OrganisationsUseCaseTests
    {
        private OrganisationsUseCase _classUnderTest;
        private Mock<IOrganisationsGateway> _mockOrganisationsGateway;

        [SetUp]
        public void Setup()
        {
            _mockOrganisationsGateway = new Mock<IOrganisationsGateway>();
            _classUnderTest = new OrganisationsUseCase(_mockOrganisationsGateway.Object);
        }

        [TestCase(TestName = "A call to the organisations use case create method calls the gateway create action")]
        public void CreateOrganisationUseCaseCallsGatewayCreateOrganisation()
        {
            var requestParams = Randomm.Create<OrganisationRequest>();
            _classUnderTest.ExecuteCreate(requestParams);
            _mockOrganisationsGateway.Verify(u => u.CreateOrganisation(It.IsAny<Organization>()), Times.Once);
        }

        [TestCase(TestName = "Given an organisation domain object is provided the created organisation is returned from the gateway")]
        public void ReturnsOrganisation()
        {
            var requestParams = Randomm.Create<OrganisationRequest>();
            var domainData = requestParams.ToDomain();
            _mockOrganisationsGateway.Setup(g => g.CreateOrganisation(It.IsAny<Organization>())).Returns(domainData);
            var expectedResponse = domainData.ToResponse();
            var response = _classUnderTest.ExecuteCreate(requestParams);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
