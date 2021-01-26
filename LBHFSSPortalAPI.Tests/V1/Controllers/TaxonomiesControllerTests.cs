using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Controllers;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class TaxonomiesControllerTests
    {
        private TaxonomiesController _classUnderTest;
        private Mock<ITaxonomyUseCase> _mockUseCase;

        public TaxonomiesControllerTests()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Cookie"] = $"access_token=xxxxx";
            _mockUseCase = new Mock<ITaxonomyUseCase>();
            _classUnderTest = new TaxonomiesController(_mockUseCase.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        #region Create Taxonomy

        [TestCase(TestName = "When the taxonomies controller CreateTaxonomy action is called the TaxonomyUseCase ExecuteCreate method is called once with data provided")]
        public void CreateTaxonomyControllerActionCallsTheTaxonomyUseCase()
        {
            var requestParams = Randomm.Create<TaxonomyRequest>();
            _classUnderTest.CreateTaxonomy(requestParams);
            _mockUseCase.Verify(uc => uc.ExecuteCreate(It.Is<TaxonomyRequest>(p => p == requestParams)), Times.Once);
        }

        [TestCase(TestName = "When the taxonomies controller CreateTaxonomy action is called and the taxonomie gets created it returns a response with a status code")]
        public void ReturnsResponseWithStatus()
        {
            var expected = Randomm.Create<TaxonomyResponse>();
            var reqParams = Randomm.Create<TaxonomyRequest>();
            _mockUseCase.Setup(u => u.ExecuteCreate(It.IsAny<TaxonomyRequest>())).Returns(expected);
            var response = _classUnderTest.CreateTaxonomy(reqParams) as CreatedResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(201);
            response.Value.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region Get Taxonomy
        [TestCase(TestName = "When the taxomony controller GetTaxonomy action is called the TaxonomyUseCase ExecuteGet method is called once with id provided")]
        public void GetTaxonomyControllerActionCallsTheTaxonomyUseCase()
        {
            var requestParam = Randomm.Create<int>();
            _classUnderTest.GetTaxonomy(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteGetById(It.Is<int>(p => p == requestParam)), Times.Once);
        }

        [TestCase(TestName = "When the taxonomy controller GetTaxonomy action is called with an id, if matched an taxonomy with a 200 status code is returned")]
        public void ReturnsGetResponseWith200Status()
        {
            var expected = Randomm.Create<TaxonomyResponse>();
            var requestParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteGetById(It.Is<int>(p => p == requestParam))).Returns(expected);
            var response = _classUnderTest.GetTaxonomy(requestParam) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase(TestName = "When the taxonomy controller Gettaxonomy action is called with an id, if not matched an error response is returned with a 404 status code")]
        public void ReturnsGetResponseWith404Status()
        {
            var reqParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteGetById(It.IsAny<int>())).Returns((TaxonomyResponse) null);
            var response = _classUnderTest.GetTaxonomy(reqParam) as NotFoundObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(404);
        }

        #endregion

        #region GetTaxonomies

        [TestCase(TestName = "When the taxomony controller ListTaxonomies action is called with a vocabulary id the TaxonomyUseCase ExecuteGet method is called once with id provided")]
        public void ListTaxonomiesControllerActionCallsTheTaxonomyUseCaseWhenIntProvided()
        {
            var requestParam = Randomm.Create<int>();
            _classUnderTest.ListTaxonomies(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteGet(It.Is<int?>(p => p == requestParam)), Times.Once);
        }

        [TestCase(TestName = "When the taxomony controller ListTaxonomies action is called with no vocabulary id the TaxonomyUseCase ExecuteGet method is called once")]
        public void ListTaxonomiesControllerActionCallsTheTaxonomyUseCaseWhenNoIntProvided()
        {
            _classUnderTest.ListTaxonomies(null);
            _mockUseCase.Verify(uc => uc.ExecuteGet(It.Is<int?>(p => p == null)), Times.Once);
        }

        [TestCase(TestName = "When the taxonomy controller ListTaxonomies action is called with a vocabulary id, if matched an List of taxonomies with a 200 status code is returned")]
        public void ReturnsListResponseWith200Status()
        {
            var expected = new TaxonomyResponseList()
            {
                Categories = Randomm.CreateMany<TaxonomyResponse>().ToList(),
                Demographics = Randomm.CreateMany<TaxonomyResponse>().ToList()
            };
            Randomm.CreateMany<TaxonomyResponse>().ToList();
            var requestParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteGet(It.Is<int?>(p => p == requestParam))).Returns(expected);
            var response = _classUnderTest.ListTaxonomies(requestParam) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }

        [TestCase(TestName = "When the taxonomy controller ListTaxonomies action is called with no vocabulary id, if matched an List of taxonomies with a 200 status code is returned")]
        public void ReturnsListAllResponseWith200Status()
        {
            var expected = new TaxonomyResponseList()
            {
                Categories = Randomm.CreateMany<TaxonomyResponse>().ToList(),
                Demographics = Randomm.CreateMany<TaxonomyResponse>().ToList()
            };
            _mockUseCase.Setup(u => u.ExecuteGet(It.Is<int?>(p => p == null))).Returns(expected);
            var response = _classUnderTest.ListTaxonomies(null) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region Delete Taxonomy
        [TestCase(TestName = "When the Taxonomy controller DeleteTaxonomy action is called, the TaxonomysUseCase ExecuteDelete method is called once with id provided")]
        public void DeleteTaxonomyControllerActionCallsTheTaxonomysUseCase()
        {
            var requestParam = Randomm.Create<int>();
            _classUnderTest.DeleteTaxonomy(requestParam);
            _mockUseCase.Verify(uc => uc.ExecuteDelete(It.Is<int>(p => p == requestParam)), Times.Once);
        }

        [TestCase(TestName = "When the Taxonomy controller DeleteTaxonomy action is called with an id and the Taxonomy gets deleted, a 200 status code is returned")]
        public void ReturnsEmptyResponseWith200Status()
        {
            var requestParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.Is<int>(p => p == requestParam)));
            var response = _classUnderTest.DeleteTaxonomy(requestParam) as OkResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
        }

        [TestCase(TestName = "When the Taxonomy controller DeleteTaxonomy action is called with an id, if not matched an error response, it is returned with a 400 status code")]
        public void ReturnsEmptyResponseWith400Status()
        {
            var reqParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.IsAny<int>())).Throws<InvalidOperationException>();
            var response = _classUnderTest.DeleteTaxonomy(reqParam) as BadRequestObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
        }

        [TestCase(TestName = "When the taxonomy controller DeleteTaxonomy action is called with a taxonomy id which is still linked to a service, a 400 error response is returned")]
        public void ExistingServiceTaxonomyLinksRespondsWith400Status()
        {
            var reqParam = Randomm.Create<int>();
            _mockUseCase.Setup(u => u.ExecuteDelete(It.IsAny<int>())).Throws<ServiceTaxonomyExistsException>();
            var response = _classUnderTest.DeleteTaxonomy(reqParam) as BadRequestObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
        }

        #endregion

        #region Patch Taxonomy
        [TestCase(TestName = "When the Taxonomy controller PatchTaxonomy action is called, the TaxonomysUseCase ExecutePatch method is called once with data and id provided")]
        public void PatchTaxonomyControllerActionCallsTheTaxonomysUseCase()
        {
            var requestParams = Randomm.Create<TaxonomyRequest>();
            var requestId = Randomm.Id();
            _classUnderTest.PatchTaxonomy(requestId, requestParams);
            _mockUseCase.Verify(uc => uc.ExecutePatch(It.Is<int>(p => p == requestId), It.Is<TaxonomyRequest>(p => p == requestParams)), Times.Once);
        }

        [TestCase(TestName = "When the Taxonomy controller PatchTaxonomy action is called and the Taxonomy gets patched it returns a response with a status code")]
        public void PatchReturnsResponseWithStatus()
        {
            var expected = Randomm.Create<TaxonomyResponse>();
            var id = Randomm.Id();
            var reqParams = Randomm.Create<TaxonomyRequest>();
            _mockUseCase.Setup(u => u.ExecutePatch(It.IsAny<int>(), It.IsAny<TaxonomyRequest>())).Returns(expected);
            var response = _classUnderTest.PatchTaxonomy(id, reqParams) as OkObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}
