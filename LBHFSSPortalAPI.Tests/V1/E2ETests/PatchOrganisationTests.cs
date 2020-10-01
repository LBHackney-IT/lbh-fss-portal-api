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
    public class PatchOrganisationTests : IntegrationTests<Startup>
    {
        [TestCase(TestName = "Given that valid parameters are provided, the specified organization is updated in the database")]
        public async Task PatchOrganisationUpdatesOrganisation()
        {
            DatabaseContext.Database.RollbackTransaction();
            var organisation = EntityHelpers.CreateOrganization();
            DatabaseContext.Organizations.Add(organisation);
            DatabaseContext.SaveChanges();
            var updOrganisation = new OrganisationRequest();
            updOrganisation.Name = Randomm.Text();
            updOrganisation.ReviewerMessage = null; // we are assuming null means no change to the record
            var organizationString = JsonConvert.SerializeObject(updOrganisation);
            HttpContent patchContent = new StringContent(organizationString, Encoding.UTF8, "application/json");
            var requestUri = new Uri($"api/v1/organisations/{organisation.Id}", UriKind.Relative);
            var response = await Client.PatchAsync(requestUri, patchContent).ConfigureAwait(false);
            patchContent.Dispose();
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            var organisationId = deserializedBody.Id;
            var dbOrganisation = DatabaseContext.Organizations.Find(organisationId);
            dbOrganisation.Should().NotBeNull();
            dbOrganisation.Name.Should().Be(organisation.Name);
            dbOrganisation.ReviewerMessage.Should().Be(organisation.ReviewerMessage);  //should not be set to null if not changed
            DatabaseContext.Database.BeginTransaction();
        }
    }
}
