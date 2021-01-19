using System;
using System.Linq;
using AutoMapper;
using Bogus.DataSets;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class AnalyticsGatewayTests : DatabaseTests
    {
        private AnalyticsGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new AnalyticsGateway(DatabaseContext);
        }

        //return all analytics events for a given organisation id
        [TestCase(TestName = "Given an organisation id when the gateway is called with the id the gateway will return analytics events that match that organisation id")]
        public void GivenAnIdAllMatchingAnalyticsEventsGetReturned()
        {
            var analyticsEvents = EntityHelpers.CreateAnalyticsEvents().ToList();
            var org = analyticsEvents[0].Service.Organisation;
            analyticsEvents[1].Service.Organisation = org;
            analyticsEvents[1].Service.OrganisationId = org.Id;
            DatabaseContext.AddRange(analyticsEvents);
            DatabaseContext.SaveChanges();
            var analyticsEventQuery = new AnalyticsEventQuery
            {
                OrganisationId = org.Id
            };
            var gatewayResult = _classUnderTest.GetAnalyticsEvents(analyticsEventQuery);
            var expectedResult = DatabaseContext.AnalyticsEvents.Where(x => x.Service.OrganisationId == analyticsEventQuery.OrganisationId);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.Service);
                return options;
            });
            gatewayResult.Count.Should().Be(2);
        }

        //return all analytics events within a given time frame for a given organisation id
        [TestCase(TestName = "Given an organisation id and date range when the gateway is called with the id the gateway will return analytics events that match that organisation id")]
        public void GivenAnIdAndDateRangeAllMatchingAnalyticsEventsGetReturned()
        {
            var analyticsEvents = EntityHelpers.CreateAnalyticsEvents().ToList();
            var org = analyticsEvents[0].Service.Organisation;
            analyticsEvents[1].Service.Organisation = org;
            analyticsEvents[1].Service.OrganisationId = org.Id;
            analyticsEvents[2].Service.Organisation = org;
            analyticsEvents[2].Service.OrganisationId = org.Id;
            analyticsEvents[0].TimeStamp = new DateTime(2020, 4, 11, 11, 0, 0);
            analyticsEvents[1].TimeStamp = new DateTime(2020, 4, 11, 11, 0, 0);
            analyticsEvents[2].TimeStamp = new DateTime(2021, 4, 11, 11, 0, 0);
            var startDate = new DateTime(2020, 4, 11, 10, 0, 0);
            var endDate = new DateTime(2020, 4, 11, 13, 0, 0);
            DatabaseContext.AddRange(analyticsEvents);
            DatabaseContext.SaveChanges();
            var analyticsEventQuery = new AnalyticsEventQuery
            {
                OrganisationId = org.Id,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            var gatewayResult = _classUnderTest.GetAnalyticsEvents(analyticsEventQuery);
            var expectedResult = DatabaseContext.AnalyticsEvents
                .Where(x => x.Service.OrganisationId == analyticsEventQuery.OrganisationId)
                .Where(x => x.TimeStamp >= analyticsEventQuery.StartDateTime)
                .Where(x => x.TimeStamp <= analyticsEventQuery.EndDateTime);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.Service);
                return options;
            });
            gatewayResult.Count.Should().Be(2);
        }

        //return all analytics events after a given start date for a given organisation id
        [TestCase(TestName = "Given an organisation id and start date when the gateway is called with the id the gateway will return analytics events that match that organisation id and date")]
        public void GivenAnIdAndStartDateAllMatchingAnalyticsEventsGetReturned()
        {
            var analyticsEvents = EntityHelpers.CreateAnalyticsEvents().ToList();
            var org = analyticsEvents[0].Service.Organisation;
            analyticsEvents[1].Service.Organisation = org;
            analyticsEvents[1].Service.OrganisationId = org.Id;
            analyticsEvents[2].Service.Organisation = org;
            analyticsEvents[2].Service.OrganisationId = org.Id;
            analyticsEvents[0].TimeStamp = new DateTime(2020, 4, 11, 11, 0, 0);
            analyticsEvents[1].TimeStamp = new DateTime(2020, 4, 11, 11, 0, 0);
            analyticsEvents[2].TimeStamp = new DateTime(2020, 1, 11, 11, 0, 0);
            var startDate = new DateTime(2020, 4, 11, 10, 0, 0);
            DatabaseContext.AddRange(analyticsEvents);
            DatabaseContext.SaveChanges();
            var analyticsEventQuery = new AnalyticsEventQuery
            {
                OrganisationId = org.Id,
                StartDateTime = startDate
            };
            var gatewayResult = _classUnderTest.GetAnalyticsEvents(analyticsEventQuery);
            var expectedResult = DatabaseContext.AnalyticsEvents
                .Where(x => x.Service.OrganisationId == analyticsEventQuery.OrganisationId)
                .Where(x => x.TimeStamp >= analyticsEventQuery.StartDateTime);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.Service);
                return options;
            });
            gatewayResult.Count.Should().Be(2);
        }

        //return all analytics events after a given end date for a given organisation id
        [TestCase(TestName = "Given an organisation id and end date when the gateway is called with the id the gateway will return analytics events that match that organisation id and date")]
        public void GivenAnIdAndEndDateAllMatchingAnalyticsEventsGetReturned()
        {
            var analyticsEvents = EntityHelpers.CreateAnalyticsEvents().ToList();
            var org = analyticsEvents[0].Service.Organisation;
            analyticsEvents[1].Service.Organisation = org;
            analyticsEvents[1].Service.OrganisationId = org.Id;
            analyticsEvents[2].Service.Organisation = org;
            analyticsEvents[2].Service.OrganisationId = org.Id;
            analyticsEvents[0].TimeStamp = new DateTime(2020, 4, 11, 11, 0, 0);
            analyticsEvents[1].TimeStamp = new DateTime(2020, 4, 12, 11, 0, 0);
            analyticsEvents[2].TimeStamp = new DateTime(2021, 1, 11, 11, 0, 0);
            var endDate = new DateTime(2020, 4, 12, 10, 0, 0);
            DatabaseContext.AddRange(analyticsEvents);
            DatabaseContext.SaveChanges();
            var analyticsEventQuery = new AnalyticsEventQuery
            {
                OrganisationId = org.Id,
                EndDateTime = endDate
            };
            var gatewayResult = _classUnderTest.GetAnalyticsEvents(analyticsEventQuery);
            var expectedResult = DatabaseContext.AnalyticsEvents
                .Where(x => x.Service.OrganisationId == analyticsEventQuery.OrganisationId)
                .Where(x => x.TimeStamp <= analyticsEventQuery.EndDateTime);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.Service);
                return options;
            });
            gatewayResult.Count.Should().Be(1);
        }
    }
}
