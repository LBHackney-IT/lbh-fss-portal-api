using System.Linq;
using AutoFixture;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    public class GetAllUseCaseTests
    {
        private Mock<IUsersGateway> _mockGateway;
        private GetAllUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IUsersGateway>();
            _classUnderTest = new GetAllUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Test]
        public void GetsAllFromTheGateway()
        {
            //var stubbedEntities = _fixture.CreateMany<UserDomain>().ToList();
            //_mockGateway.Setup(x => x.GetAllUsers()).Returns(stubbedEntities.ToList());

            //var expectedResponse = new ResponseObjectList { ResponseObjects = stubbedEntities.ToResponse() };

            //_classUnderTest.Execute().Should().BeEquivalentTo(expectedResponse);

            var stubbedUsers = _fixture
                .Build<UserDomain>()
                //.Without(contact => contact.Contacts)
                .CreateMany();

            _mockGateway.Setup(x =>
                    x.GetAllUsers())
                .Returns(stubbedUsers.ToList());

            var userQueryParam = new UserQueryParam
            {
                //FirstName = "ciasom",
                //LastName = "tessellate"
            };

            var response = _classUnderTest.Execute(userQueryParam);

            response.Should().NotBeNull();
            response.Users.Should().BeEquivalentTo(stubbedUsers.ToResponse());
        }

        //TODO: Add extra tests here for extra functionality added to the use case
    }
}
