using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class TaxonomyUseCaseTests
    {
        private TaxonomyUseCase _classUnderTest;
        private Mock<ITaxonomyGateway> _mockTaxonomyGateway;

        [SetUp]
        public void Setup()
        {
            _mockTaxonomyGateway = new Mock<ITaxonomyGateway>();
            _classUnderTest = new TaxonomyUseCase(_mockTaxonomyGateway.Object);
        }

        #region Create Taxonomy
        [TestCase(TestName = "A call to the taxonomy use case create method calls the gateway create action")]
        public void CreateTaxonomyUseCaseCallsGatewayCreateTaxonomy()
        {
            var requestParams = Randomm.Create<TaxonomyRequest>();
            _classUnderTest.ExecuteCreate(requestParams);
            _mockTaxonomyGateway.Verify(u => u.CreateTaxonomy(It.IsAny<Taxonomy>()), Times.Once);
        }

        [TestCase(TestName = "Given an taxonomy object is provided the created taxonomy domain is returned")]
        public void ReturnsCreatedTaxonomy()
        {
            var requestParams = Randomm.Create<TaxonomyRequest>();
            var domainData = requestParams.ToDomain();
            _mockTaxonomyGateway.Setup(g => g.CreateTaxonomy(It.IsAny<Taxonomy>())).Returns(domainData);
            var expectedResponse = domainData.ToResponse();
            var response = _classUnderTest.ExecuteCreate(requestParams);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        #endregion

        #region Get Taxonomy
        [TestCase(TestName = "A call to the taxonomy use case get method calls the gateway get action")]
        public void GetTaxonomyUseCaseCallsGatewayGetTaxonomy()
        {
            var id = Randomm.Create<int>();
            _classUnderTest.ExecuteGetById(id);
            _mockTaxonomyGateway.Verify(u => u.GetTaxonomy(It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid taxonomy id is provided a matching taxonomy is returned")]
        public void ReturnsOrganisation()
        {
            var taxonomy = Randomm.Create<TaxonomyDomain>();
            var id = Randomm.Create<int>();
            _mockTaxonomyGateway.Setup(g => g.GetTaxonomy(It.IsAny<int>())).Returns(taxonomy);
            var expectedResponse = taxonomy.ToResponse();
            var response = _classUnderTest.ExecuteGetById(id);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Get Taxonomies
        [TestCase(TestName = "A call to the taxonomies use case get all method calls the gateway get action")]
        public void GetTaxonomiesUseCaseCallsGatewayGetTaxonomies()
        {
            _classUnderTest.ExecuteGetAll();
            _mockTaxonomyGateway.Verify(u => u.GetAllTaxonomies(), Times.Once);
        }

        [TestCase(TestName = "Given call to get all taxonomies all taxonomies are returned")]
        public void ReturnsTaxonomies()
        {
            var taxonomies = Randomm.CreateMany<TaxonomyDomain>().ToList();
            _mockTaxonomyGateway.Setup(g => g.GetAllTaxonomies()).Returns(taxonomies);
            var expectedResponse = taxonomies.ToResponse();
            var response = _classUnderTest.ExecuteGetAll();
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion


        #region Get Taxonomies by vocabulary
        [TestCase(TestName = "A call to the taxonomies by vocab use case get all method calls the gateway get action")]
        public void GetTaxonomiesByVocabUseCaseCallsGatewayGetTaxonomiesByVocab()
        {
            var vocabularyId = Randomm.Create<int>();
            _classUnderTest.ExecuteGetByVocabulary(vocabularyId);
            _mockTaxonomyGateway.Verify(u => u.GetTaxonomiesByVocabulary(It.IsAny<string>()), Times.Once);
        }

        [TestCase(TestName = "Given call to get taxonomies by vocab all matching taxonomies are returned")]
        public void ReturnsTaxonomiesByVocab()
        {
            var taxonomies = Randomm.CreateMany<TaxonomyDomain>().ToList();
            _mockTaxonomyGateway.Setup(g => g.GetTaxonomiesByVocabulary(It.IsAny<string>())).Returns(taxonomies);
            var expectedResponse = taxonomies.ToResponse();
            var response = _classUnderTest.ExecuteGetByVocabulary(1);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Delete Organisation
        [TestCase(TestName = "A call to the taxonomy use case delete method calls the gateway delete action")]
        public void DeleteTaxonomyUseCaseCallsGatewayDeleteTaxonomy()
        {
            var id = Randomm.Create<int>();
            _classUnderTest.ExecuteDelete(id);
            _mockTaxonomyGateway.Verify(u => u.DeleteTaxonomy(It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid taxonomy id is provided a if an taxonomy is deleted then no exception is thrown")]
        public void CallsGatewayDeleteAndDoesNotThrowErrorIfSuccessful()
        {
            var id = Randomm.Create<int>();
            _mockTaxonomyGateway.Setup(g => g.DeleteTaxonomy(It.IsAny<int>()));
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().NotThrow();
        }

        [TestCase(TestName = "Given an taxonomy id is provided if no taxomony is matched then an exception is thrown")]
        public void CallsGatewayDeleteAndThrowsErrorIfNotSuccessful()
        {
            var id = Randomm.Create<int>();
            _mockTaxonomyGateway.Setup(g => g.DeleteTaxonomy(It.IsAny<int>())).Throws<InvalidOperationException>();
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().Throw<InvalidOperationException>();
        }
        #endregion

        #region Patch Taxonomy
        [TestCase(TestName = "A call to the taxonomies use case patch method calls the gateway patch action")]
        public void PatchTaxonomyUseCaseCallsGatewayPatchTaxonomy()
        {
            var taxonomy = Randomm.Create<TaxonomyRequest>();
            var requestId = Randomm.Id();
            _mockTaxonomyGateway.Setup(gw => gw.PatchTaxonomy(It.IsAny<int>(),It.IsAny<Taxonomy>())).Returns(taxonomy.ToDomain());
            _classUnderTest.ExecutePatch(requestId, taxonomy);
            _mockTaxonomyGateway.Verify(u => u.PatchTaxonomy(It.IsAny<int>(),It.IsAny<Taxonomy>()), Times.Once);
        }
        #endregion
    }
}
