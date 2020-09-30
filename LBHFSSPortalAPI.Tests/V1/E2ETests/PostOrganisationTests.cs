using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.E2ETests
{
    [TestFixture]
    public class PostOrganisationTests : IntegrationTests<Startup>
    {
        [TestCase(TestName = "Given that valid parameters are provided, organizations are added to the database")]
        public async Task PostOrganisationCreatesOrganisation()
        {
            var organization = Randomm.Create<OrganisationRequest>();
            var organizationString = JsonConvert.SerializeObject(organization);
            HttpContent postContent = new StringContent(organizationString, Encoding.UTF8, "application/json");
            var requestUri = new Uri("api/v1/organisations", UriKind.Relative);
            var response = await Client.PostAsync(requestUri, postContent).ConfigureAwait(false);
            postContent.Dispose();
            response.StatusCode.Should().Be(201);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            var organisationId = deserializedBody.Id;
            var dbOrganisation = DatabaseContext.Organizations.Find(organisationId);
            dbOrganisation.Should().NotBeNull();
            // Change to dbOrganization to a response object.  A factory is needed.
            dbOrganisation.Should().BeEquivalentTo(deserializedBody);
        }
    }
}
