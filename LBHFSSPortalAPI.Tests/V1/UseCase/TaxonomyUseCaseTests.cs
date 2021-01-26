using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
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
            requestParams.VocabularyId = 1;
            _classUnderTest.ExecuteCreate(requestParams);
            _mockTaxonomyGateway.Verify(u => u.CreateTaxonomy(It.IsAny<Taxonomy>()), Times.Once);
        }

        [TestCase(TestName = "Given an taxonomy object is provided the created taxonomy domain is returned")]
        public void ReturnsCreatedTaxonomy()
        {
            var requestParams = Randomm.Create<TaxonomyRequest>();
            requestParams.VocabularyId = 1;
            var domainData = requestParams.ToDomain();
            _mockTaxonomyGateway.Setup(g => g.CreateTaxonomy(It.IsAny<Taxonomy>())).Returns(domainData);
            var expectedResponse = domainData.ToResponse();
            var response = _classUnderTest.ExecuteCreate(requestParams);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [TestCase(TestName = "Given an invalid vocabulary id then an exception is thrown on POST")]
        public void CallsGatewayPostAndThrowsErrorIfInvalidVocabularyIdProvided()
        {
            var taxonomy = Randomm.Create<TaxonomyRequest>();
            taxonomy.VocabularyId = 0;
            var requestId = Randomm.Id(); ;
            _mockTaxonomyGateway.Setup(gw => gw.CreateTaxonomy(It.IsAny<Taxonomy>())).Returns(taxonomy.ToDomain());
            _classUnderTest.Invoking(c => c.ExecuteCreate(taxonomy)).Should().Throw<InvalidOperationException>();
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
            _classUnderTest.ExecuteGet(null);
            _mockTaxonomyGateway.Verify(u => u.GetAllTaxonomies(), Times.Once);
        }

        [TestCase(TestName = "Given call to get all taxonomies all taxonomies are returned")]
        public void ReturnsTaxonomies()
        {
            var taxonomies = Randomm.CreateMany<TaxonomyDomain>().ToList();
            taxonomies[0].Vocabulary = "category";
            taxonomies[0].Vocabulary = "demographic";
            taxonomies[0].Vocabulary = "category";
            _mockTaxonomyGateway.Setup(g => g.GetAllTaxonomies()).Returns(taxonomies);
            var expectedResponse = new TaxonomyResponseList()
            {
                Categories = taxonomies.Where(x => x.Vocabulary == "category").ToList().ToResponse(),
                Demographics = taxonomies.Where(x => x.Vocabulary == "demographic").ToList().ToResponse()
            };
            var response = _classUnderTest.ExecuteGet(null);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion


        #region Get Taxonomies by vocabulary
        [TestCase(TestName = "A call to the taxonomies by vocab use case get all method calls the gateway get action")]
        public void GetTaxonomiesByVocabUseCaseCallsGatewayGetTaxonomiesByVocab()
        {
            var vocabularyId = Randomm.Create<int>();
            _classUnderTest.ExecuteGet(vocabularyId);
            _mockTaxonomyGateway.Verify(u => u.GetTaxonomiesByVocabulary(It.IsAny<string>()), Times.Once);
        }

        [TestCase(TestName = "Given call to get taxonomies by vocab all matching taxonomies are returned")]
        public void ReturnsTaxonomiesByVocab()
        {
            var taxonomies = Randomm.CreateMany<TaxonomyDomain>().ToList();
            taxonomies[0].Vocabulary = "category";
            taxonomies[0].Vocabulary = "demographic";
            taxonomies[0].Vocabulary = "category";
            _mockTaxonomyGateway.Setup(g => g.GetTaxonomiesByVocabulary(It.IsAny<string>())).Returns(taxonomies);
            var expectedResponse = new TaxonomyResponseList()
            {
                Categories = taxonomies.Where(x => x.Vocabulary == "category").ToList().ToResponse(),
                Demographics = new List<TaxonomyResponse>()
            };
            var response = _classUnderTest.ExecuteGet(1);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);
        }
        #endregion

        #region Delete Taxonomy
        [TestCase(TestName = "A call to the taxonomy use case delete method calls the gateway delete action")]
        public void DeleteTaxonomyUseCaseCallsGatewayDeleteTaxonomy()
        {
            var serviceTaxonomies = new List<ServiceTaxonomyDomain>();
            _mockTaxonomyGateway.Setup(g => g.GetServiceTaxonomies(It.IsAny<int>())).Returns(serviceTaxonomies);
            var id = Randomm.Create<int>();
            _classUnderTest.ExecuteDelete(id);
            _mockTaxonomyGateway.Verify(u => u.DeleteTaxonomy(It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given a valid taxonomy id is provided a if an taxonomy is deleted then no exception is thrown")]
        public void CallsGatewayDeleteAndDoesNotThrowErrorIfSuccessful()
        {
            var id = Randomm.Create<int>();
            var serviceTaxonomies = new List<ServiceTaxonomyDomain>();
            _mockTaxonomyGateway.Setup(g => g.GetServiceTaxonomies(It.IsAny<int>())).Returns(serviceTaxonomies);
            _mockTaxonomyGateway.Setup(g => g.DeleteTaxonomy(It.IsAny<int>()));
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().NotThrow();
        }

        [TestCase(TestName = "Given an taxonomy id is provided if no taxomony is matched then an exception is thrown")]
        public void CallsGatewayDeleteAndThrowsErrorIfNotSuccessful()
        {
            var id = Randomm.Create<int>();
            var serviceTaxonomies = new List<ServiceTaxonomyDomain>();
            _mockTaxonomyGateway.Setup(g => g.GetServiceTaxonomies(It.IsAny<int>())).Returns(serviceTaxonomies);
            _mockTaxonomyGateway.Setup(g => g.DeleteTaxonomy(It.IsAny<int>())).Throws<InvalidOperationException>();
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().Throw<InvalidOperationException>();
        }

        [TestCase(TestName = "A call to the taxonomy use case delete method calls the gateway get servicetaxonomies action")]
        public void DeleteTaxonomyUseCaseCallsGatewayGetServiceTaxonomies()
        {
            var id = Randomm.Create<int>();
            var serviceTaxonomies = new List<ServiceTaxonomyDomain>();
            _mockTaxonomyGateway.Setup(g => g.GetServiceTaxonomies(It.IsAny<int>())).Returns(serviceTaxonomies);
            _classUnderTest.ExecuteDelete(id);
            _mockTaxonomyGateway.Verify(u => u.GetServiceTaxonomies(It.IsAny<int>()), Times.Once);
        }

        [TestCase(TestName = "Given an taxonomy id is provided if matching servicetaxonomies exist then an exception is thrown")]
        public void DeleteTaxonomyWhereTaxonomyStillLinkedToServiceThrowsError()
        {
            var id = Randomm.Create<int>();
            var serviceTaxonomies = Randomm.CreateMany<ServiceTaxonomyDomain>().ToList();
            _mockTaxonomyGateway.Setup(g => g.GetServiceTaxonomies(It.IsAny<int>())).Returns(serviceTaxonomies);
            _classUnderTest.Invoking(c => c.ExecuteDelete(id)).Should().Throw<ServiceTaxonomyExistsException>();
        }

        [TestCase(TestName = "Given an taxonomy id is provided if matching servicetaxonomies exist then an exception is thrown and returns services")]
        public void DeleteTaxonomyWhereTaxonomyStillLinkedToServiceThrowsErrorAndReturnsListOfServices()
        {
            var id = Randomm.Create<int>();
            var serviceTaxonomies = Randomm.CreateMany<ServiceTaxonomyDomain>().ToList();
            _mockTaxonomyGateway.Setup(g => g.GetServiceTaxonomies(It.IsAny<int>())).Returns(serviceTaxonomies);
            _classUnderTest.Invoking(c => c.ExecuteDelete(id))
                .Should()
                .Throw<ServiceTaxonomyExistsException>()
                .And
                .Services.Count.IsSameOrEqualTo(serviceTaxonomies.Count);
        }

        #endregion

        #region Patch Taxonomy
        [TestCase(TestName = "A call to the taxonomies use case patch method calls the gateway patch action")]
        public void PatchTaxonomyUseCaseCallsGatewayPatchTaxonomy()
        {
            var taxonomy = Randomm.Create<TaxonomyRequest>();
            taxonomy.VocabularyId = 1;
            var requestId = Randomm.Id();
            _mockTaxonomyGateway.Setup(gw => gw.PatchTaxonomy(It.IsAny<int>(), It.IsAny<Taxonomy>())).Returns(taxonomy.ToDomain());
            _classUnderTest.ExecutePatch(requestId, taxonomy);
            _mockTaxonomyGateway.Verify(u => u.PatchTaxonomy(It.IsAny<int>(), It.IsAny<Taxonomy>()), Times.Once);
        }
        [TestCase(TestName = "Given an invalid vocabulary id then an exception is thrown on PATCH")]
        public void CallsGatewayPatchAndThrowsErrorIfInvalidVocabularyIdProvided()
        {
            var taxonomy = Randomm.Create<TaxonomyRequest>();
            taxonomy.VocabularyId = 3;
            var requestId = Randomm.Id(); ;
            _mockTaxonomyGateway.Setup(gw => gw.PatchTaxonomy(It.IsAny<int>(), It.IsAny<Taxonomy>())).Returns(taxonomy.ToDomain());
            _classUnderTest.Invoking(c => c.ExecutePatch(requestId, taxonomy)).Should().Throw<InvalidOperationException>();
        }
        #endregion
    }
}
