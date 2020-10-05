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
            var organisation = EntityHelpers.CreateOrganisation();
            var gatewayResult = _classUnderTest.CreateOrganisation(organisation);
            var expectedResult = DatabaseContext.Organisations.Where(x => x.Name == organisation.Name).FirstOrDefault();
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganisations);
                return options;
            });
        }

        [TestCase(TestName = "Given an organisation id when the gateway is called with the id the gateway will return an organisation that matches")]
        public void GivenAnIdAMatchingOrganisationGetsReturned()
        {
            var organisation = EntityHelpers.CreateOrganisation();
            DatabaseContext.Add(organisation);
            DatabaseContext.SaveChanges();
            var gatewayResult = _classUnderTest.GetOrganisation(organisation.Id);
            var expectedResult = DatabaseContext.Organisations.Find(organisation.Id);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganisations);
                return options;
            });
        }

        [TestCase(TestName = "Given an organisation id when the gateway is called with the id the gateway will delete the organisation that matches")]
        public void GivenAnIdAMatchingOrganisationGetsDeleted()
        {
            var organisation = EntityHelpers.CreateOrganisation();
            DatabaseContext.Add(organisation);
            DatabaseContext.SaveChanges();
            _classUnderTest.DeleteOrganisation(organisation.Id);
            var expectedResult = DatabaseContext.Organisations.Find(organisation.Id);
            expectedResult.Should().BeNull();
        }

        [TestCase(TestName = "Given an organisation object when the gateway is called with the object the gateway will update the organisation that matches")]
        public void GivenAnOrganisationAMatchingOrganisationGetsUpdated()
        {
            var organisation = EntityHelpers.CreateOrganisation();
            DatabaseContext.Add(organisation);
            DatabaseContext.SaveChanges();
            var organisationDomain = _mapper.ToDomain(organisation);
            organisationDomain.Name = Randomm.Text();
            _classUnderTest.PatchOrganisation(organisationDomain);
            var expectedResult = DatabaseContext.Organisations.Find(organisation.Id);
            expectedResult.Should().BeEquivalentTo(organisationDomain, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganisations);
                return options;
            });
        }

        [TestCase(TestName = "Given search parameters, when the gateway is called with the gateway will return a organisations that match")]
        public void GivenSearchParametersMatchingOrganisationsGetReturned()
        {
            var organisations = EntityHelpers.CreateOrganisations(10).ToList();
            DatabaseContext.AddRange(organisations);
            DatabaseContext.SaveChanges();
            var searchParams = new OrganisationSearchRequest();
            var orgToFind = organisations.First();
            searchParams.Search = orgToFind.Name;
            searchParams.Sort = "Name";
            searchParams.Direction = SortDirection.Asc.ToString();
            var gatewayResult = _classUnderTest.SearchOrganisations(searchParams).Result;
            gatewayResult.Should().NotBeNull();
            gatewayResult.First().Should().BeEquivalentTo(orgToFind, options =>
            {
                options.Excluding(ex => ex.ReviewerU);
                options.Excluding(ex => ex.Services);
                options.Excluding(ex => ex.UserOrganisations);
                return options;
            });
        }
    }
}
