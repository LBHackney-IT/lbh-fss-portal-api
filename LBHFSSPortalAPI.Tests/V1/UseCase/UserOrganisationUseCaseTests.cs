using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UserOrganisationUseCaseTests
    {
        private UserOrganisationLinksUseCase _classUnderTest;
        private Mock<IUserOrganisationGateway> _mockUserOrganisationLinksGateway;

        [SetUp]
        public void Setup()
        {
            _mockUserOrganisationLinksGateway = new Mock<IUserOrganisationGateway>();
            _classUnderTest = new UserOrganisationLinksUseCase(_mockUserOrganisationLinksGateway.Object);
        }

        #region Create UserOrganisation
        [TestCase(TestName = "A call to the user organisation links use case create method calls the gateway create action")]
        public void CreateUserOrganisationLinksUseCaseCallsGatewayCreateUserOrganisationLink()
        {
            var requestParams = Randomm.Create<UserOrganisationRequest>();
            var domainData = Randomm.Create<UserOrganisationDomain>();
            _mockUserOrganisationLinksGateway.Setup(x => x.LinkUserToOrganisation(It.IsAny<int>(), It.IsAny<int>())).Returns(domainData);
            _classUnderTest.ExecuteCreate(requestParams);
            _mockUserOrganisationLinksGateway.Verify(u => u.LinkUserToOrganisation(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given required request object is provided the created userorganisation is returned")]
        public void ReturnsCreatedUserOrganisation()
        {
            var domainData = Randomm.Create<UserOrganisationDomain>();
            var requestParams = new UserOrganisationRequest
            {
                OrganisationId = domainData.OrganisationId,
                UserId = domainData.UserId
            };
            _mockUserOrganisationLinksGateway.Setup(x => x.LinkUserToOrganisation(requestParams.OrganisationId, requestParams.UserId)).Returns(domainData);
            var response = _classUnderTest.ExecuteCreate(requestParams);
            response.Should().NotBeNull();
            response.OrganisationId.Should().Be(requestParams.OrganisationId);
            response.UserId.Should().Be(requestParams.UserId);
        }
        #endregion

        #region Delete UserOrganisation
        [TestCase(TestName = "A call to the userorganisations use case delete method calls the gateway delete action")]
        public void DeleteUserOrganisationUseCaseCallsGatewayDeleteUserOrganisation()
        {
            var id = Randomm.Create<int>();
            _classUnderTest.ExecuteDelete(id);
            _mockUserOrganisationLinksGateway.Verify(u => u.DeleteUserOrganisationLink(It.IsAny<int>()), Times.Once);
        }
        #endregion
    }
}
