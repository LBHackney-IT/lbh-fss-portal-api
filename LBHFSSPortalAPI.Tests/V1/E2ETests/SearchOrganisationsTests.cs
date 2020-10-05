using System;
using System.Linq;
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
    public class SearchOrganisationsTests : IntegrationTests<Startup>
    {
        [TestCase(TestName = "Given valid serach parameters provided, organisations that match are returned")]
        public async Task SearchOrganisationBySearchParamsReturnsOrganisations()
        {
            DatabaseContext.Database.RollbackTransaction();
            var organisations = EntityHelpers.CreateOrganisations(10);
            var searchParams = organisations.First().Name;
            DatabaseContext.Organisations.AddRange(organisations);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations?search={searchParams}", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            deserializedBody.Should().NotBeNull();
            deserializedBody.Should().BeEquivalentTo(organisations.First());
            DatabaseContext.Database.BeginTransaction();
        }
    }
}
