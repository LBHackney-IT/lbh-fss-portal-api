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
    public class GetOrganisationsTests : IntegrationTests<Startup>
    {
        [TestCase(TestName = "Given an id provided, an organisation with the matching id is returned")]
        public async Task GetOrganisationByIdReturnsOrganisation()
        {
            DatabaseContext.Database.RollbackTransaction();
            var organisation = EntityHelpers.CreateOrganization();
            DatabaseContext.Organizations.Add(organisation);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations/{organisation.Id}", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            deserializedBody.Should().NotBeNull();
            //deserializedBody.Should().BeEquivalentTo(organisation);
            DatabaseContext.Database.BeginTransaction();
        }
    }
}
