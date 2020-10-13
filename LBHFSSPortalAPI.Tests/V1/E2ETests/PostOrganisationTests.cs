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
        [TestCase(TestName = "Given that valid parameters are provided, organisations are added to the database")]
        public async Task PostOrganisationCreatesOrganisation()
        {
            DatabaseContext.Database.RollbackTransaction();
            var session = EntityHelpers.CreateSession("Admin");
            DatabaseContext.Sessions.Add(session);
            Client.DefaultRequestHeaders.Add("Cookie", $"access_token={session.Payload}");
            DatabaseContext.SaveChanges();
            var organisation = EntityHelpers.CreatePostOrganisation();
            var organisationString = JsonConvert.SerializeObject(organisation);
            HttpContent postContent = new StringContent(organisationString, Encoding.UTF8, "application/json");
            var requestUri = new Uri("api/v1/organisations", UriKind.Relative);
            var response = await Client.PostAsync(requestUri, postContent).ConfigureAwait(false);
            postContent.Dispose();
            response.StatusCode.Should().Be(201);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            var organisationId = deserializedBody.Id;
            var dbOrganisation = DatabaseContext.Organisations.Find(organisationId);
            dbOrganisation.Should().NotBeNull();
            DatabaseContext.Database.BeginTransaction();
        }
    }
}
