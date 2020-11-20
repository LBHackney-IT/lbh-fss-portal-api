using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class UserOrganisationLinksGatewayTests : DatabaseTests
    {
        private UserOrganisationLinksGateway _classUnderTest;
        private OrganisationsGateway _organisationsGateway;
        private UsersGateway _usersGateway;
        private MappingHelper _mapper = new MappingHelper();
        private OrganisationDomain _organisationDomain;
        private UserDomain _userDomain;
        private Organisation _organisation;
        private User _user;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new UserOrganisationLinksGateway(DatabaseContext);
            _organisationsGateway = new OrganisationsGateway(DatabaseContext);
            _usersGateway = new UsersGateway(DatabaseContext);
        }

        private Organisation CreateOrganisation()
        {
            _organisation = EntityHelpers.CreateOrganisation();
            _organisationDomain = _organisationsGateway.CreateOrganisation(_organisation);
            _organisation = _mapper.FromDomain(_organisationDomain);
            return _organisation;
        }

        private User CreateUser()
        {
            _user = EntityHelpers.CreateUser();
            _userDomain = _usersGateway.AddUser(_mapper.ToDomain(_user));
            _user = _mapper.FromDomain(_userDomain);
            return _user;
        }

        [TestCase(TestName = "Given an organisation and user when the gateway is called the gateway will create the userorganisation")]
        public void GivenOrganisationAndUserUserOrganisationGetsCreated()
        {
            var gatewayResult = _classUnderTest.LinkUserToOrganisationAsync(CreateOrganisation().Id, CreateUser().Id);
            var expectedResult = DatabaseContext.UserOrganisations.Where(x => x.UserId == _user.Id && x.OrganisationId == _organisation.Id).FirstOrDefault();
            gatewayResult.Should().NotBeNull();
            gatewayResult.Result.Should().BeEquivalentTo(_mapper.ToDomain(expectedResult), options =>
            {
                options.Excluding(ex => ex.Organisation);
                options.Excluding(ex => ex.User);
                options.Excluding(ex => ex.CreatedAt);
                return options;
            });
        }

        [TestCase(TestName = "Given a userid when the gateway is called with the id the gateway will delete the userorganisation that matches")]
        public void GivenMatchingUserIdUserOrganisationGetsDeleted()
        {
            var createdLink = _classUnderTest.LinkUserToOrganisationAsync(CreateOrganisation().Id, CreateUser().Id);
            _classUnderTest.DeleteUserOrganisationLink(_user.Id);
            var expectedResult = DatabaseContext.UserOrganisations.Where(x => x.UserId == _user.Id).FirstOrDefault();
            expectedResult.Should().BeNull();
        }

        [TestCase(TestName = "Given a UserId that does not exist when the Gateway is called the gateway will throw an exception on create")]
        public void GivenInvalidUserIdOnCreateExceptionWillBeThrown()
        {
            _user = EntityHelpers.CreateUser();
            _user.Id = 999999999;
            _classUnderTest.Invoking(c => c.LinkUserToOrganisationAsync(CreateOrganisation().Id, _user.Id))
                .Should()
                .Throw<Exception>();
        }

        [TestCase(TestName = "Given a OrganisationId that does not exist when the Gateway is called the gateway will throw an exception on create")]
        public void GivenInvalidOrganisationIdOnCreateExceptionWillBeThrown()
        {
            _organisation = EntityHelpers.CreateOrganisation();
            _organisation.Id = 999999999;
            _classUnderTest.Invoking(c => c.LinkUserToOrganisationAsync(_organisation.Id, CreateUser().Id))
                .Should()
                .Throw<Exception>();
        }

        [TestCase(TestName = "Given a User organisation link that does not have the provided UserId when the Gateway is called the gateway will throw an exception on create")]
        public void GivenNoUserOrganisationLinkExistsForProvidedUserIdOnDeleteExceptionWillBeThrown()
        {
            _classUnderTest.Invoking(c => c.DeleteUserOrganisationLink(999999))
                .Should()
                .Throw<Exception>()
                .WithMessage("User Organisation Link does not exist");
        }
    }
}
