using System.Linq;
using AutoMapper;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class OrganisationsGatewayTests : DatabaseTests
    {
        private OrganisationsGateway _classUnderTest;
        private MappingHelper _mapper = new MappingHelper();

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new OrganisationsGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given a organisation domain object when the gateway is called the gateway will create the organisation")]
        public void GivenOrganisationDomainObjectOrganisationGetsCreated()
        {
            var organisation = EntityHelpers.CreateOrganization();
            var gatewayResult = _classUnderTest.CreateOrganisation(organisation);
            var expectedResult = DatabaseContext.Organizations.Where(x => x.Name == organisation.Name).FirstOrDefault();
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganizations);
                return options;
            });
        }

        [TestCase(TestName = "Given an organisation id when the gateway is called with the id the gateway will return an organisation that matches")]
        public void GivenAnIdAMatchingOrganisationGetsReturned()
        {
            var organisation = EntityHelpers.CreateOrganization();
            DatabaseContext.Add(organisation);
            DatabaseContext.SaveChanges();
            var gatewayResult = _classUnderTest.GetOrganisation(organisation.Id);
            var expectedResult = DatabaseContext.Organizations.Find(organisation.Id);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganizations);
                return options;
            });
        }

        [TestCase(TestName = "Given an organisation id when the gateway is called with the id the gateway will delete the organisation that matches")]
        public void GivenAnIdAMatchingOrganisationGetsDeleted()
        {
            var organisation = EntityHelpers.CreateOrganization();
            DatabaseContext.Add(organisation);
            DatabaseContext.SaveChanges();
            _classUnderTest.DeleteOrganisation(organisation.Id);
            var expectedResult = DatabaseContext.Organizations.Find(organisation.Id);
            expectedResult.Should().BeNull();
        }

        [TestCase(TestName = "Given an organisation object when the gateway is called with the object the gateway will update the organisation that matches")]
        public void GivenAnOrganisationAMatchingOrganisationGetsUpdated()
        {
            var organisation = EntityHelpers.CreateOrganization();
            DatabaseContext.Add(organisation);
            DatabaseContext.SaveChanges();
            var organisationDomain = _mapper.ToDomain(organisation);
            organisationDomain.Name = Randomm.Text();
            _classUnderTest.PatchOrganisation(organisationDomain);
            var expectedResult = DatabaseContext.Organizations.Find(organisation.Id);
            expectedResult.Should().BeEquivalentTo(organisationDomain, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganizations);
                return options;
            });
        }

    }
}
