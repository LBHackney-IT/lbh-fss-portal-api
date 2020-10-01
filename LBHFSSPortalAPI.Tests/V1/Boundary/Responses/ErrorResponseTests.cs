using System;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Response;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Boundary.Requests
{
    [TestFixture]
    public class ErrorResponseTests
    {
        [Test]
        public void GivenNoParametersWhenErrorResponseConstructorIsCalledThenItInitializesErrorsFieldToEmptyList()
        {
            //act
            var errorResponse = new ErrorResponse();

            //assert
            errorResponse.Errors.Should().NotBeNull();
            errorResponse.Errors.Count.Should().Be(0);
        }

        [Test]
        public void GivenAnyNumberOfErrorStringParametersWhenErrorResponseConstructorIsCalledThenItInitializesErrorsFieldToAListOfErrorStringsCorrespondingToPassedInParameters()
        {
            //act
            Func<string> error = () => { return Randomm.Text(); };

            var singleParameterErrorResponse = new ErrorResponse(error());
            var fourParameterErrorResponse = new ErrorResponse(error(), error(), error(), error());
            var sevenParameterErrorResponse = new ErrorResponse(error(), error(), error(), error(), error(), error(), error());

            //assert
            singleParameterErrorResponse.Errors.Count.Should().Be(1);
            fourParameterErrorResponse.Errors.Count.Should().Be(4);
            sevenParameterErrorResponse.Errors.Count.Should().Be(7);
        }
    }
}
