using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class AnalyticsUseCaseTests
    {
        private AnalyticsUseCase _classUnderTest;
        private Mock<IAnalyticsGateway> _mockAnalyticsGateway;

        [SetUp]
        public void Setup()
        {
            _mockAnalyticsGateway = new Mock<IAnalyticsGateway>();
            _classUnderTest = new AnalyticsUseCase(_mockAnalyticsGateway.Object);
        }

        #region Get Analytics Events
        [TestCase(TestName = "A call to the analytics events use case get method calls the gateway get action")]
        public void GetAnalyticsUseCaseCallsGatewayGetAnalyticsEvents()
        {
            var query = Randomm.Create<AnalyticsRequest>();
            _classUnderTest.ExecuteGet(query);
            _mockAnalyticsGateway.Verify(u => u.GetAnalyticsEvents(It.IsAny<AnalyticsEventQuery>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid organisation id is provided all matching events are returned")]
        public void GetAnalyticsEventsReturnsMatchingAnalyticsEvents()
        {
            var analyticsEvents = Randomm.CreateMany<AnalyticsEventDomain>().ToList();
            var service = Randomm.Create<ServiceDomain>();
            service.OrganisationId = 123;
            analyticsEvents[0].Service = service;
            analyticsEvents[1].Service = service;
            analyticsEvents[2].Service = service;

            var analyticsRequest = new AnalyticsRequest
            {
                OrganisationId = 123
            };

            _mockAnalyticsGateway.Setup(g => g.GetAnalyticsEvents(It.IsAny<AnalyticsEventQuery>())).Returns(analyticsEvents);
            var expectedResponse = analyticsEvents.ToResponse();
            var response = _classUnderTest.ExecuteGet(analyticsRequest);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
            response.AnalyticEvents.Count.Should().Be(3);
        }
        #endregion

    }
}
