using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Infrastructure;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.E2ETests
{
    [TestFixture]
    public class SearchOrganisationsTests : IntegrationTests<Startup>
    {
        [TestCase(TestName = "Given valid search parameters provided, organisations that match are returned")]
        public async Task SearchOrganisationBySearchParamsReturnsOrganisations()
        {
            DatabaseContext.Database.RollbackTransaction();
            var organisations = EntityHelpers.CreateOrganisations(10);
            var searchParam = organisations.First().Name;
            DatabaseContext.Organisations.AddRange(organisations);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations?search={searchParam}&sort=name&direction=asc", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(true);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponseList>(stringResponse);
            deserializedBody.Should().NotBeNull();
            deserializedBody.Organisations.First().Name.Should().BeEquivalentTo(organisations.First().Name);
        }

        [TestCase(TestName = "Given valid search parameters and a specified sort order, organisations that match are returned in the order specified")]
        public async Task SearchOrganisationBySearchParamsReturnsOrganisationsInTheSortOrderSpecified()
        {
            DatabaseContext.Database.RollbackTransaction();
            var rand = new Random();
            var organisations = EntityHelpers.CreateOrganisations(10).ToList();
            var searchParam = Randomm.Word();
            var first = rand.Next(10, 20);
            var second = rand.Next(20, 30);
            organisations[5].Name = searchParam + first;
            organisations[3].Name = searchParam + second;
            DatabaseContext.Organisations.AddRange(organisations);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations?search={searchParam}&sort=name&direction=asc", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(true);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponseList>(stringResponse);
            deserializedBody.Should().NotBeNull();
            deserializedBody.Organisations[0].Name.Should().BeEquivalentTo(organisations[5].Name);
            deserializedBody.Organisations[1].Name.Should().BeEquivalentTo(organisations[3].Name);
            DatabaseContext.Database.BeginTransaction();
        }
    }
}
