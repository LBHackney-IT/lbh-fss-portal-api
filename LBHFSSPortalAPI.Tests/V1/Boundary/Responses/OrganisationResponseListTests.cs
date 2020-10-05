using System.Collections.Generic;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Response;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Boundary.Requests
{
    public class OrganisationResponseListTests
    {
                [TestCase(TestName = "Organisation response object should have the correct properties")]
        public void GetServiceResponseObjectShouldHaveCorrectProperties()
        {
            var entityType = typeof(OrganisationResponseList);
            entityType.GetProperties().Length.Should().Be(1);
            var entity = Randomm.Create<OrganisationResponseList>();
            Assert.That(entity, Has.Property("Organisations").InstanceOf(typeof(List<OrganisationResponse>)));
        }
    }
}
