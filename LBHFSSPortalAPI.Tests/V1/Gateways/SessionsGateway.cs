using System;
using System.Linq;
using AutoMapper;
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
    public class SessionsGatewayTests : DatabaseTests
    {
        private SessionsGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new SessionsGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given a session in the database, when the session id is provided the session data is returned")]
        public void GivenValidSessionInDatabaseSessionGetsReturned()
        {
            var session = EntityHelpers.CreateSession("VCSO");
            DatabaseContext.Sessions.Add(session);
            DatabaseContext.SaveChanges();
            var gatewayResult = _classUnderTest.GetSessionByToken(session.Payload);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(session);
        }

        [TestCase(TestName = "Given a session in the database, when the refresh session is called the last access date is updated")]
        public void GivenValidSessionInDatabaseRefreshSessionUpdatesLastAccessAt()
        {
            var session = EntityHelpers.CreateSession("VCSO");
            DatabaseContext.Sessions.Add(session);
            DatabaseContext.SaveChanges();
            _classUnderTest.RefreshSessionExpiry(session.Id);
            var updatedSession = DatabaseContext.Sessions.Find(session.Id);
            updatedSession.LastAccessAt.Should().BeCloseTo(DateTime.Now);
        }
    }
}
