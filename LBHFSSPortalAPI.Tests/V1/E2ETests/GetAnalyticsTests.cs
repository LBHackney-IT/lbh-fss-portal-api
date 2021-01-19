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
    public class GetAnalyticsTests : IntegrationTests<Startup>
    {
        [SetUp]
        public void SetUp()
        {
            CustomizeAssertions.ApproximationDateTime();
            DatabaseContext.Database.RollbackTransaction();
            E2ETestHelpers.ClearTable(DatabaseContext);
        }

        [TestCase(TestName = "Given an organisation id provided, Analytics Events with the matching ids are returned")]
        public async Task GetOrganisationByIdReturnsOrganisation()
        {
            var session = EntityHelpers.CreateSession("Admin");
            DatabaseContext.Sessions.Add(session);
            Client.DefaultRequestHeaders.Add("Cookie", $"access_token={session.Payload}");

            var analyticsEvents = EntityHelpers.CreateAnalyticsEvents().ToList();
            var org = analyticsEvents[0].Service.Organisation;
            analyticsEvents[1].Service.Organisation = org;
            analyticsEvents[1].Service.OrganisationId = org.Id;
            DatabaseContext.AddRange(analyticsEvents);
            DatabaseContext.SaveChanges();

            var requestUri = new Uri($"api/v1/analytics-event/?organisationid={org.Id}", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(true);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<AnalyticsResponseList>(stringResponse);
            deserializedBody.Should().NotBeNull();
        }

        [TestCase(TestName =
            "Given a session token is provided for an expired session, a not authorised response should be returned")]
        public async Task GetanalyticsEventWithExpiredSessionReturnsNotAuthorised()
        {
            var session = EntityHelpers.CreateSession("Admin");
            session.LastAccessAt = DateTime.Today.AddDays(-2);
            DatabaseContext.Sessions.Add(session);
            Client.DefaultRequestHeaders.Add("Cookie", $"access_token={session.Payload}");
            var analyticsEvents = EntityHelpers.CreateAnalyticsEvents().ToList();
            var org = analyticsEvents[0].Service.Organisation;
            analyticsEvents[1].Service.Organisation = org;
            analyticsEvents[1].Service.OrganisationId = org.Id;
            DatabaseContext.AddRange(analyticsEvents);
            DatabaseContext.SaveChanges();

            var requestUri = new Uri($"api/v1/analytics-event/?{org.Id}", UriKind.Relative);
            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            response.StatusCode.Should().Be(401);
            DatabaseContext.Sessions.Count(s => s.UserId == session.UserId).Should().Be(0);
        }
    }
}
