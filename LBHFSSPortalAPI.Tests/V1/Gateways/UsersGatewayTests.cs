using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class UsersGatewayTests : DatabaseTests
    {
        private UsersGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new UsersGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given list of users in the database users are returned")]
        public async Task GivenUsersInDatabaseUsersGetsReturned()
        {
            var count = 5;
            var users = EntityHelpers.CreateUsers(count);
            var queryParams = new UserQueryParam
            {
                Sort = "name",
                Direction = "asc"
            };
            await DatabaseContext.Users.AddRangeAsync(users).ConfigureAwait(true);
            await DatabaseContext.SaveChangesAsync().ConfigureAwait(true);
            var gatewayResult = await _classUnderTest.GetAllUsers(queryParams).ConfigureAwait(true);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Count.Should().Be(count);
        }

        [TestCase(TestName = "Given list of users in the database users with status of deleted are not returned")]
        public async Task GivenUsersInDatabaseUsersWithStatusOfDeletedDoNotGetReturned()
        {
            var count = 5;
            var users = EntityHelpers.CreateUsers(count);
            users.First().Status = "deleted";
            var queryParams = new UserQueryParam
            {
                Sort = "name",
                Direction = "asc"
            };
            await DatabaseContext.Users.AddRangeAsync(users).ConfigureAwait(true);
            await DatabaseContext.SaveChangesAsync().ConfigureAwait(true);
            var gatewayResult = await _classUnderTest.GetAllUsers(queryParams).ConfigureAwait(true);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Count.Should().Be(count - 1);
        }

    }
}
