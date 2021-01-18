using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
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
    public class UpdateServicesUseCaseTests
    {
        private UpdateServiceUseCase _classUnderTest;
        private Mock<IServicesGateway> _mockServicesGateway;
        private Mock<IAddressXRefGateway> _mockAddressXRefGateway;

        [SetUp]
        public void Setup()
        {
            _mockServicesGateway = new Mock<IServicesGateway>();
            _mockAddressXRefGateway = new Mock<IAddressXRefGateway>();
            _classUnderTest = new UpdateServiceUseCase(_mockServicesGateway.Object,
                _mockAddressXRefGateway.Object);
        }

        [TestCase(TestName = "A call to the update service use case create method calls the gateway update action")]
        public async Task UpdateServiceUseCaseCallsGatewayUpdateService()
        {
            var requestParams = Randomm.Create<ServiceRequest>();
            var locationRequest = Randomm.Create<ServiceLocationRequest>();
            requestParams.Locations = new List<ServiceLocationRequest>() { locationRequest };
            var domainData = requestParams.ToDomain();
            domainData.Id = 123;
            domainData.Organisation = Randomm.Create<OrganisationDomain>();
            var accessToken = Randomm.Word();
            _mockServicesGateway.Setup(g => g.UpdateService(It.IsAny<ServiceRequest>(), 123)).ReturnsAsync(domainData);
            var myResult = await _classUnderTest.Execute(requestParams, 123).ConfigureAwait(false);
            _mockServicesGateway.Verify(u => u.UpdateService(It.IsAny<ServiceRequest>(), It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "A call to the update service use case update method with location calls the address xref gateway get action")]
        public async Task UpdateServiceUseCaseWithLocationCallsAddressXrefGateway()
        {
            var requestParams = Randomm.Create<ServiceRequest>();
            var orgDomain = Randomm.Create<OrganisationDomain>();
            requestParams.OrganisationId = orgDomain.Id;
            var locationRequest = Randomm.Create<ServiceLocationRequest>();
            requestParams.Locations = new List<ServiceLocationRequest>() { locationRequest };
            var serviceDomainData = requestParams.ToDomain();
            serviceDomainData.Organisation = orgDomain;
            serviceDomainData.Id = 123;
            var accessToken = Randomm.Word();
            _mockServicesGateway.Setup(g => g.UpdateService(It.IsAny<ServiceRequest>(), 123)).ReturnsAsync(serviceDomainData);
            _mockAddressXRefGateway.Setup(g => g.GetNHSNeighbourhood(It.IsAny<string>())).Returns("NHS1");
            var response = await _classUnderTest.Execute(requestParams, 123).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(serviceDomainData, options =>
            {
                options.Excluding(ex => ex.ImageId);
                options.Excluding(ex => ex.ServiceTaxonomies);
                options.Excluding(ex => ex.Organisation);
                options.Excluding(ex => ex.ServiceLocations);
                return options;
            });
            response.Locations.FirstOrDefault().Address1.Should().Be(serviceDomainData.ServiceLocations.FirstOrDefault().Address1);
            response.Locations.FirstOrDefault().Address2.Should().Be(serviceDomainData.ServiceLocations.FirstOrDefault().Address2);
            response.Locations.FirstOrDefault().City.Should().Be(serviceDomainData.ServiceLocations.FirstOrDefault().City);
            response.Locations.FirstOrDefault().Country.Should().Be(serviceDomainData.ServiceLocations.FirstOrDefault().Country);
            response.Locations.FirstOrDefault().NHSNeighbourhood.Should().Be(serviceDomainData.ServiceLocations.FirstOrDefault().NHSNeighbourhood);
        }
    }
}
