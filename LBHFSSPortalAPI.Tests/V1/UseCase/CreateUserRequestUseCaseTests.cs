using AutoFixture;
using FluentAssertions;
using LBHFSSPortalAPI.V1.Boundary.Requests;
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
            var strResponse = "test";
            _mockAuthenticateGateway.Setup(s => s.CreateUser(It.IsAny<UserCreateRequest>())).Returns(strResponse);
            var dataToSave = new Fixture().Build<UserCreateRequest>().Create();
            var response = _classUnderTest.Execute(dataToSave);
            response.SubId.Should().BeSameAs(strResponse);
        }
    }
}
