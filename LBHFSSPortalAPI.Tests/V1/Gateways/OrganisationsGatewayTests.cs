using System.Linq;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class OrganisationsGatewayTests : DatabaseTests
    {
        private OrganisationsGateway _classUnderTest;

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
    }
}
