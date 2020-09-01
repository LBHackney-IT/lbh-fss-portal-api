using AutoFixture;
using FluentAssertions;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class CreateUserRequestUseCaseTests
    {
        private Mock<IAuthenticateGateway> _mockAuthenticateGateway;
        private Mock<IUsersGateway> _mockUsersGateway;
        private CreateUserRequestUseCase _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _mockAuthenticateGateway = new Mock<IAuthenticateGateway>();
            _mockUsersGateway = new Mock<IUsersGateway>();
            _classUnderTest = new CreateUserRequestUseCase(_mockAuthenticateGateway.Object, _mockUsersGateway.Object);
        }

        [Test]
        public void ExecuteSavesRequestToDatabase()
        {
            var dataToSave = new Fixture().Build<UserCreateRequest>().Create();
            var recData = new UserDomain { Email = dataToSave.Email, Name = dataToSave.Name, Status = "Pending", Id = 1 };
            _mockUsersGateway.Setup(u => u.AddUser(It.IsAny<UserDomain>())).Returns(recData);
            var response = _classUnderTest.Execute(dataToSave);
            response.Should().BeEquivalentTo(recData);
        }
    }
}
