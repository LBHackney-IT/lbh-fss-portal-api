using System;
using AutoFixture;
using FluentAssertions;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Controllers;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private AuthenticateController _classUnderTest;
        private Mock<ICreateUserRequestUseCase> _fakeCreateUserRequestUseCase;
        private Mock<IAuthenticateUseCase> _fakeAuthenticateUseCase;
        private Mock<IConfirmUserUseCase> _fakeConfirmUserUseCase;

        [SetUp]
        public void SetUp()
        {
            _fakeCreateUserRequestUseCase = new Mock<ICreateUserRequestUseCase>();
            _fakeAuthenticateUseCase = new Mock<IAuthenticateUseCase>();
            _fakeConfirmUserUseCase = new Mock<IConfirmUserUseCase>();

            _classUnderTest = new AuthenticateController(
                _fakeAuthenticateUseCase.Object,
                _fakeCreateUserRequestUseCase.Object,
                _fakeConfirmUserUseCase.Object);
        }

        [Test]
        public void CreateUserReturnsCreatedResponseWithStatus()
        {
            var request = new Fixture().Build<UserCreateRequest>().Create();
            _fakeCreateUserRequestUseCase.Setup(x => x.Execute(request))
                .Returns(new UserResponse { Email = request.Email, Name = request.Name, Status = "unverified" });
            var response = _classUnderTest.CreateUser(request) as CreatedResult;
            response.StatusCode.Should().Be(201);
        }

        [Test]
        public void CreateUserWithInvalidParamReturnsBadRequestResponse()
        {
            var request = new Fixture().Build<UserCreateRequest>().Create();
            request.Email = null;
            _fakeCreateUserRequestUseCase.Setup(x => x.Execute(request))
                .Throws(new InvalidOperationException());
            var response = _classUnderTest.CreateUser(request) as BadRequestObjectResult;
            response.StatusCode.Should().Be(400);
        }

    }
}
