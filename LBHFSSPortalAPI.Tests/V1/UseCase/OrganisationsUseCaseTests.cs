using System;
using System.Collections.Generic;
using System.Linq;
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
        private Mock<IUsersGateway> _mockUsersGateway;
        private Mock<INotifyGateway> _mockNotifyGateway;

        [SetUp]
        public void Setup()
        {
            _mockOrganisationsGateway = new Mock<IOrganisationsGateway>();
            _mockUsersGateway = new Mock<IUsersGateway>();
            _mockNotifyGateway = new Mock<INotifyGateway>();
            _classUnderTest = new OrganisationsUseCase(_mockOrganisationsGateway.Object, _mockNotifyGateway.Object, _mockUsersGateway.Object);
        }

        #region Create Organisation
        [TestCase(TestName = "A call to the organisations use case create method calls the gateway create action")]
        public void CreateOrganisationUseCaseCallsGatewayCreateOrganisation()
        {
            var requestParams = Randomm.Create<OrganisationRequest>();
            _classUnderTest.ExecuteCreate(requestParams);
            _mockOrganisationsGateway.Verify(u => u.CreateOrganisation(It.IsAny<Organisation>()), Times.Once);
        }

        //[TestCase(TestName = "Given an organisation domain object is provided the created organisation is returned")]
        public void ReturnsCreatedOrganisation()
        {
            var requestParams = Randomm.Create<OrganisationRequest>();
            var domainData = requestParams.ToDomain();
            var users = new List<UserDomain> {Randomm.Create<UserDomain>()};
            _mockOrganisationsGateway.Setup(g => g.CreateOrganisation(It.IsAny<Organisation>())).Returns(domainData);
            _mockUsersGateway.Setup(ug => ug.GetAllUsers(It.IsAny<UserQueryParam>())).ReturnsAsync(users);
            var expectedResponse = domainData.ToResponse();
            var response = _classUnderTest.ExecuteCreate(requestParams);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        #endregion

        #region Get Organisation
        [TestCase(TestName = "A call to the organisations use case get method calls the gateway get action")]
        public void GetOrganisationUseCaseCallsGatewayGetOrganisation()
        {
            var id = Randomm.Create<int>();
            _classUnderTest.ExecuteGet(id);
            _mockOrganisationsGateway.Verify(u => u.GetOrganisation(It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid organisation id is provided a matching organisation is returned")]
        public void ReturnsOrganisation()
        {
            var organisation = Randomm.Create<OrganisationDomain>();
            var id = Randomm.Create<int>();
            _mockOrganisationsGateway.Setup(g => g.GetOrganisation(It.IsAny<int>())).Returns(organisation);
            var expectedResponse = organisation.ToResponse();
            var response = _classUnderTest.ExecuteGet(id);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Delete Organisation
        [TestCase(TestName = "A call to the organisations use case delete method calls the gateway delete action")]
        public void DeleteOrganisationUseCaseCallsGatewayDeleteOrganisation()
        {
            var id = Randomm.Create<int>();
            _classUnderTest.ExecuteDelete(id);
            _mockOrganisationsGateway.Verify(u => u.DeleteOrganisation(It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid organisation id is provided a if an organisation is deleted then no exception is thrown")]
        public void CallsGatewayDeleteAndDoesNotThrowErrorIfSuccessful()
        {
            var id = Randomm.Create<int>();
            _mockOrganisationsGateway.Setup(g => g.DeleteOrganisation(It.IsAny<int>()));
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().NotThrow();
        }

        [TestCase(TestName = "Given an organisation id is provided if no organisation is matched then an exception is thrown")]
        public void CallsGatewayDeleteAndThrowsErrorIfNotSuccessful()
        {
            var id = Randomm.Create<int>();
            _mockOrganisationsGateway.Setup(g => g.DeleteOrganisation(It.IsAny<int>())).Throws<InvalidOperationException>();
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().Throw<InvalidOperationException>();
        }
        #endregion

        #region Patch Organisation
        [TestCase(TestName = "A call to the organisations use case patch method calls the gateway patch action")]
        public void PatchOrganisationUseCaseCallsGatewayPatchOrganisation()
        {
            var organisation = Randomm.Create<OrganisationRequest>();
            var requestId = Randomm.Id();
            _mockOrganisationsGateway.Setup(gw => gw.GetOrganisation(It.IsAny<int>())).Returns(organisation.ToDomain());
            _mockOrganisationsGateway.Setup(gw => gw.PatchOrganisation(It.IsAny<OrganisationDomain>())).Returns(organisation.ToDomain());
            _classUnderTest.ExecutePatch(requestId, organisation);
            _mockOrganisationsGateway.Verify(u => u.GetOrganisation(It.IsAny<int>()), Times.Once);
            _mockOrganisationsGateway.Verify(u => u.PatchOrganisation(It.IsAny<OrganisationDomain>()), Times.Once);
        }
        #endregion

        #region Search Organisation
        [TestCase(TestName = "A call to the organisations use case get method with search params calls the gateway search action")]
        public void GetOrganisationUseCaseWithSearchParamsCallsGatewaySearchOrganisation()
        {
            var searchParams = Randomm.Create<OrganisationSearchRequest>();
            _classUnderTest.ExecuteGet(searchParams);
            _mockOrganisationsGateway.Verify(u => u.SearchOrganisations(It.IsAny<OrganisationSearchRequest>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid search parameters are provided matching organisations are returned")]
        public void ReturnsOrganisations()
        {
            var organisations = Randomm.CreateMany<OrganisationDomain>(10).ToList();
            var searchParams = Randomm.Create<OrganisationSearchRequest>();
            _mockOrganisationsGateway.Setup(g => g.SearchOrganisations(It.IsAny<OrganisationSearchRequest>())).ReturnsAsync(organisations);
            var expectedResponse = organisations.ToResponse();
            var response = _classUnderTest.ExecuteGet(searchParams).Result;
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion
    }
}
