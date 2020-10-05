using System.Linq;
using AutoFixture;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.UseCase;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    public class GetAllUsersUseCaseTests
    {
        private Mock<IUsersGateway> _mockGateway;
        private GetAllUsersUseCase _getAllUsersUseCase;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IUsersGateway>();
            _getAllUsersUseCase = new GetAllUsersUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Test]
        public void GetsAllFromTheGateway()
        {
            var stubbedUsers = _fixture
                    .Build<UserDomain>()
                    .Without(u => u.Organizations)
                    .Without(u => u.UserRoles)
                    .CreateMany();

            var userQueryParam = new UserQueryParam()
            {
                Search = string.Empty,
                Direction = "asc",
                Sort = "name",
            };

            _mockGateway.Setup(x => x.GetAllUsers(userQueryParam))
                .ReturnsAsync(stubbedUsers.ToList());

            var response = _getAllUsersUseCase.Execute(userQueryParam).Result;

            response.Should().NotBeNull();
            response.Users.Should().BeEquivalentTo(stubbedUsers.ToResponse());
        }

        //TODO: Add extra tests here for extra functionality added to the use case
    }
}
