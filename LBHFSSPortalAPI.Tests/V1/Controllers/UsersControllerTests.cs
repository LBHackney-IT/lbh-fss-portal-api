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
        private UsersController _classUnderTest;
        private Mock<ICreateUserRequestUseCase> _fakeCreateUserRequestUseCase;
        private Mock<IGetAllUsersUseCase> _fakeGetAllUsersUseCase;
        private Mock<IConfirmUserRegUseCase> _fakeConfirmUserRegUseCase;


        [SetUp]
        public void SetUp()
        {
            _fakeCreateUserRequestUseCase = new Mock<ICreateUserRequestUseCase>();
            _fakeGetAllUsersUseCase = new Mock<IGetAllUsersUseCase>();
            _fakeConfirmUserRegUseCase = new Mock<IConfirmUserRegUseCase>();
            _classUnderTest = new UsersController(_fakeGetAllUsersUseCase.Object,
                    _fakeCreateUserRequestUseCase.Object, _fakeConfirmUserRegUseCase.Object);
        }

        [Test]
        public void CreateUserReturnsCreatedResponseWithStatus()
        {
            var request = new Fixture().Build<UserCreateRequest>().Create();
            _fakeCreateUserRequestUseCase.Setup(x => x.Execute(request))
                .Returns(new UserResponse { Email = request.EmailAddress, Name = request.Name, Status = "unverified" });
            var response = _classUnderTest.CreateUser(request) as CreatedResult;
            response.StatusCode.Should().Be(201);
        }

        [Test]
        public void CreateUserWithInvalidParamReturnsBadRequestResponse()
        {
            var request = new Fixture().Build<UserCreateRequest>().Create();
            request.EmailAddress = null;
            _fakeCreateUserRequestUseCase.Setup(x => x.Execute(request))
                .Throws(new InvalidOperationException());
            var response = _classUnderTest.CreateUser(request) as BadRequestObjectResult;
            response.StatusCode.Should().Be(400);
        }

    }
}
