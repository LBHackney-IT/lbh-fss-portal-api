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
    public class GetOrganisationsTests : IntegrationTests<Startup>
    {
        [SetUp]
        public void SetUp()
        {
            CustomizeAssertions.ApproximationDateTime();
            DatabaseContext.Database.RollbackTransaction();
            E2ETestHelpers.ClearTable(DatabaseContext);
        }

        [TestCase(TestName = "Given an id provided, an organisation with the matching id is returned")]
        public async Task GetOrganisationByIdReturnsOrganisation()
        {
            var session = EntityHelpers.CreateSession("Admin");
            DatabaseContext.Sessions.Add(session);
            Client.DefaultRequestHeaders.Add("Cookie", $"access_token={session.Payload}");
            var organisation = EntityHelpers.CreateOrganisation();
            DatabaseContext.Organisations.Add(organisation);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations/{organisation.Id}", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            deserializedBody.Should().NotBeNull();
        }

        [TestCase(TestName =
            "Given a session token is provided for an expired session, a not authorised response should be returned")]
        public async Task GetOrganisationWithExpiredSessionReturnsNotAuthorised()
        {
            var session = EntityHelpers.CreateSession("Admin");
            session.LastAccessAt = DateTime.Today.AddDays(-2);
            DatabaseContext.Sessions.Add(session);
            Client.DefaultRequestHeaders.Add("Cookie", $"access_token={session.Payload}");
            var organisation = EntityHelpers.CreateOrganisation();
            DatabaseContext.Organisations.Add(organisation);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations/{organisation.Id}", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            response.StatusCode.Should().Be(401);
            DatabaseContext.Sessions.Count(s => s.UserId == session.UserId).Should().Be(0);
        }
    }
}
