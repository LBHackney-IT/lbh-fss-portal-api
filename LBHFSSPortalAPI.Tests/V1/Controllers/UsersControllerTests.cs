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
        private Mock<IGetAllUsersUseCase> _fakeGetAllUsersUseCase;
        private Mock<IGetUserUseCase> _fakeGetUserUseCase;
        private Mock<ICreateUserRequestUseCase> _fakeCreateUserRequestUseCase;
        private Mock<IUpdateUserRequestUseCase> _fakeUpdateUserRequestUseCase;
        private Mock<IDeleteUserRequestUseCase> _fakeDeleteUserRequestUseCase;


        [SetUp]
        public void SetUp()
        {
            _fakeGetAllUsersUseCase = new Mock<IGetAllUsersUseCase>();
            _fakeGetUserUseCase = new Mock<IGetUserUseCase>();
            _fakeCreateUserRequestUseCase = new Mock<ICreateUserRequestUseCase>();
            _fakeUpdateUserRequestUseCase = new Mock<IUpdateUserRequestUseCase>();
            _fakeDeleteUserRequestUseCase = new Mock<IDeleteUserRequestUseCase>();

            _classUnderTest = new UsersController(_fakeGetAllUsersUseCase.Object,
                                                  _fakeGetUserUseCase.Object,
                                                  _fakeCreateUserRequestUseCase.Object,
                                                  _fakeUpdateUserRequestUseCase.Object,
                                                  _fakeDeleteUserRequestUseCase.Object);
        }

        // TODO: Add tests
        [Test]
        public void CreateUserReturnsCreatedResponseWithStatus()
        {

        }
    }
}
