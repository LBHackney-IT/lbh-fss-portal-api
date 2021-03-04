using System.Linq;
using AutoMapper;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    public class SynonymWordGatewayTests : DatabaseTests
    {
        private SynonymWordsGateway _classUnderTest;
        private MappingHelper _mapper = new MappingHelper();
        [SetUp]
        public void Setup()
        {
            _classUnderTest = new SynonymWordsGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given a synonymWord domain object when the gateway is called the gateway will create the synonymWord")]
        public void GivenSynonymWordDomainObjectSynonymWordGetsCreated()
        {
            var synonymWord = EntityHelpers.CreateSynonymWord();
            var gatewayResult = _classUnderTest.CreateSynonymWord(synonymWord);
            var expectedResult = DatabaseContext.SynonymWords.Where(x => x.Word == synonymWord.Word).FirstOrDefault();
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                return options;
            });
        }

        [TestCase(TestName = "Given an synonymWord id when the gateway is called with the id the gateway will return an synonymWord that matches")]
        public void GivenAnIdAMatchingSynonymWordGetsReturned()
        {
            var synonymWord = EntityHelpers.CreateSynonymWord();
            DatabaseContext.Add(synonymWord);
            DatabaseContext.SaveChanges();
            var gatewayResult = _classUnderTest.GetSynonymWord(synonymWord.Id);
            var expectedResult = DatabaseContext.SynonymWords.Find(synonymWord.Id);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                return options;
            });
        }

        [TestCase(TestName = "Given an synonymWord id when the gateway is called with the id the gateway will delete the synonymWord that matches")]
        public void GivenAnIdAMatchingSynonymWordGetsDeleted()
        {
            var synonymWord = EntityHelpers.CreateSynonymWord();
            DatabaseContext.Add(synonymWord);
            DatabaseContext.SaveChanges();
            _classUnderTest.DeleteSynonymWord(synonymWord.Id);
            var expectedResult = DatabaseContext.SynonymWords.Find(synonymWord.Id);
            expectedResult.Should().BeNull();
        }

        [TestCase(TestName = "Given an synonymWord object when the gateway is called with the object the gateway will update the synonymWord that matches")]
        public void GivenAnSynonymWordAMatchingSynonymWordGetsUpdated()
        {
            var synonymWord = EntityHelpers.CreateSynonymWord();
            DatabaseContext.Add(synonymWord);
            DatabaseContext.SaveChanges();
            var synonymWordDomain = _mapper.ToDomain(synonymWord);
            synonymWordDomain.Word = Randomm.Text();
            _classUnderTest.PatchSynonymWord(synonymWordDomain);
            var expectedResult = DatabaseContext.SynonymWords.Find(synonymWord.Id);
            expectedResult.Should().BeEquivalentTo(synonymWordDomain, options =>
            {
                return options;
            });
        }

        [TestCase(TestName = "Given search parameters, when the gateway is called with the gateway will return a synonymWords that match")]
        public void GivenSearchParametersMatchingSynonymWordsGetReturned()
        {
            var synonymWord = EntityHelpers.CreateSynonymWord();
            DatabaseContext.AddRange(synonymWord);
            DatabaseContext.SaveChanges();
            var searchParams = new SynonymWordSearchRequest();
            var sgToFind = synonymWord;
            searchParams.Search = sgToFind.Word;
            searchParams.Sort = "Word";
            searchParams.Direction = SortDirection.Asc.ToString();
            var gatewayResult = _classUnderTest.SearchSynonymWords(searchParams);
            gatewayResult.Should().NotBeNull();
            gatewayResult.First().Should().BeEquivalentTo(sgToFind, options =>
            {
                options.Excluding(ex => ex.Group);
                return options;
            });
        }
    }
}
