using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.E2ETests
{
    [TestFixture]
    public class DeleteOrganisationsTests : IntegrationTests<Startup>
    {
        [TestCase(TestName = "Given an id provided, a delete success response is returned")]
        public async Task DeleteOrganisationReturnsSuccess()
        {
            DatabaseContext.Database.RollbackTransaction();
            var session = EntityHelpers.CreateSession("Admin");
            DatabaseContext.Sessions.Add(session);
            Client.DefaultRequestHeaders.Add("Cookie", $"access_token={session.Payload}");
            var organisation = EntityHelpers.CreateOrganisation();
            DatabaseContext.Organisations.Add(organisation);
            DatabaseContext.SaveChanges();
            var requestUri = new Uri($"api/v1/organisations/{organisation.Id}", UriKind.Relative);
            var response = await Client.DeleteAsync(requestUri).ConfigureAwait(false);
            response.StatusCode.Should().Be(200);
            var content = response.Content;
            var stringResponse = await content.ReadAsStringAsync().ConfigureAwait(true);
            var deserializedBody = JsonConvert.DeserializeObject<OrganisationResponse>(stringResponse);
            deserializedBody.Should().BeNull();
            DatabaseContext.Database.BeginTransaction();
        }
    }
}
