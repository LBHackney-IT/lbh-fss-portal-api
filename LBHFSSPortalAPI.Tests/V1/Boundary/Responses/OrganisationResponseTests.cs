using System;
using System.Collections.Generic;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Boundary.Requests
{
    [TestFixture]
    public class OrganisationResponseTests
    {
        [TestCase(TestName = "Organisation response object should have the correct properties")]
        public void GetServiceResponseObjectShouldHaveCorrectProperties()
        {
            var entityType = typeof(OrganisationResponse);
            entityType.GetProperties().Length.Should().Be(37);
            var entity = Randomm.Create<OrganisationResponse>();
            Assert.That(entity, Has.Property("Id").InstanceOf(typeof(int)));
            Assert.That(entity, Has.Property("Name").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("CreatedAt").InstanceOf(typeof(DateTime)));
            Assert.That(entity, Has.Property("UpdatedAt").InstanceOf(typeof(DateTime)));
            Assert.That(entity, Has.Property("SubmittedAt").InstanceOf(typeof(DateTime)));
            Assert.That(entity, Has.Property("ReviewedAt").InstanceOf(typeof(DateTime)));
            Assert.That(entity, Has.Property("ReviewerMessage").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("Status").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("StatusMessage").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("IsHackneyBased").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("IsRegisteredCharity").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("CharityNumber").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("IsRegisteredCommunityInterestCompany").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("CommunityInterestCompanyNumber").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("HasHcOrColGrant").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("HasHcvsOrHgOrAelGrant").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("IsTraRegistered").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("RslOrHaAssociation").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("IsLotteryFunded").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("LotteryFundedProject").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("FundingOther").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("HasChildSupport").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("HasChildSafeguardingLead").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("ChildSafeguardingLeadFirstName").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("ChildSafeguardingLeadLastName").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("ChildSafeguardingLeadTrainingMonth").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("ChildSafeguardingLeadTrainingYear").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("HasAdultSupport").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("HasAdultSafeguardingLead").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("AdultSafeguardingLeadFirstName").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("AdultSafeguardingLeadLastName").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("AdultSafeguardingLeadTrainingMonth").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("AdultSafeguardingLeadTrainingYear").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("HasEnhancedSupport").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("IsLocalOfferListed").InstanceOf(typeof(bool)));
            Assert.That(entity, Has.Property("Reviewer").InstanceOf(typeof(OrganisationReviewer)));
            Assert.That(entity, Has.Property("Users").InstanceOf(typeof(List<OrgUser>)));
        }

    }
}
