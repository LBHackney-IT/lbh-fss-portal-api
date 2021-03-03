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
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class SynonymUseCaseTests
    {
        private SynonymsUseCase _classUnderTest;
        private Mock<ISynonymGroupsGateway> _mockSynonymGroupsGateway;
        private Mock<ISynonymWordsGateway> _mockSynonymWordsGateway;
        private IGoogleClient _mockGoogleClient;

        [SetUp]
        public void Setup()
        {
            _mockSynonymGroupsGateway = new Mock<ISynonymGroupsGateway>();
            _mockSynonymWordsGateway = new Mock<ISynonymWordsGateway>();
            _mockGoogleClient = new GoogleClientMocked();
            _classUnderTest = new SynonymsUseCase(_mockSynonymGroupsGateway.Object,
                _mockSynonymWordsGateway.Object,
                _mockGoogleClient);
        }

        [TestCase(TestName = "Given a valid SynonymGroup id is provided a matching SynonymGroup is returned")]
        public void ReturnsSynonymGroup()
        {
            var synonymGroup = Randomm.Create<SynonymGroupDomain>();
            var id = Randomm.Create<int>();
            _mockSynonymGroupsGateway.Setup(g => g.GetSynonymGroup(It.IsAny<int>())).Returns(synonymGroup);
            var expectedResponse = synonymGroup.ToResponse();
            var sgResponse = _classUnderTest.ExecuteGetById(id);
            sgResponse.Should().NotBeNull();
            sgResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [TestCase(TestName = "Given a valid SynonymWord id is provided a matching SynonymWord is returned")]
        public void ReturnsSynonymWord()
        {
            var synonymWord = Randomm.Create<SynonymWordDomain>();
            var id = Randomm.Create<int>();
            _mockSynonymWordsGateway.Setup(g => g.GetSynonymWord(It.IsAny<int>())).Returns(synonymWord);
            var expectedResponse = synonymWord.ToResponse();
            var sgResponse = _classUnderTest.ExecuteGetWordById(id);
            sgResponse.Should().NotBeNull();
            sgResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [TestCase(TestName = "Given a valid SynonymWord list is provided, a success message to update the synonyms is returned")]
        public void UpdateSynonymWords()
        {
            var synonymGroup = Randomm.Create<SynonymGroupDomain>();
            _mockSynonymGroupsGateway.Setup(g => g.GetSynonymGroup(It.IsAny<int>())).Returns(synonymGroup);
            var synonymWord = Randomm.Create<SynonymWordDomain>();
            _mockSynonymWordsGateway.Setup(g => g.GetSynonymWord(It.IsAny<int>())).Returns(synonymWord);
            var accessToken = "123456";
            SynonymUpdateRequest synonymUpdateRequest = new SynonymUpdateRequest()
            {
                GoogleFileId = "1V89AxVH",
                SheetName = "Synonyms",
                SheetRange = "A:AU"
            };
            var response = _classUnderTest.ExecuteUpdate(accessToken, synonymUpdateRequest);
            Assert.IsTrue(response.Result.Success == true);
        }

    }
}
