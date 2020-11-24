using System.Collections.Generic;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Boundary.Requests
{
    [TestFixture]
    public class OrganisationSearchRequestTests
    {
        [Test]
        public void GetServiceResponseObjectShouldHaveCorrectProperties()
        {
            var entityType = typeof(OrganisationSearchRequest);
            entityType.GetProperties().Length.Should().Be(6);
            var entity = Randomm.Create<OrganisationSearchRequest>();
            Assert.That(entity, Has.Property("Search").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("Status").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("Sort").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("Direction").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("Offset").InstanceOf(typeof(int)));
            Assert.That(entity, Has.Property("Limit").InstanceOf(typeof(int)));
        }
    }
}
