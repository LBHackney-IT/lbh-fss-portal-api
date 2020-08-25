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

        [SetUp]
        public void SetUp()
        {
            _fakeGetAllUsersUseCase = new Mock<IGetAllUsersUseCase>();
            _classUnderTest = new UsersController(_fakeGetAllUsersUseCase.Object);
        }

        // TODO: Add tests
        [Test]
        public void CreateUserReturnsCreatedResponseWithStatus()
        {

        }

    }
}
