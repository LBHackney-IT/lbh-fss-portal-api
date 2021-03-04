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
    public class SynonymGroupGatewayTests : DatabaseTests
    {
        private SynonymGroupsGateway _classUnderTest;
        private MappingHelper _mapper = new MappingHelper();
        [SetUp]
        public void Setup()
        {
            _classUnderTest = new SynonymGroupsGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given a synonymGroup domain object when the gateway is called the gateway will create the synonymGroup")]
        public void GivenSynonymGroupDomainObjectSynonymGroupGetsCreated()
        {
            var synonymGroup = EntityHelpers.CreateSynonymGroup();
            var gatewayResult = _classUnderTest.CreateSynonymGroup(synonymGroup);
            var expectedResult = DatabaseContext.SynonymGroups.Where(x => x.Name == synonymGroup.Name).FirstOrDefault();
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.SynonymWords);
                return options;
            });
        }

        [TestCase(TestName = "Given an synonymGroup id when the gateway is called with the id the gateway will return an synonymGroup that matches")]
        public void GivenAnIdAMatchingSynonymGroupGetsReturned()
        {
            var synonymGroup = EntityHelpers.CreateSynonymGroup();
            DatabaseContext.Add(synonymGroup);
            DatabaseContext.SaveChanges();
            var gatewayResult = _classUnderTest.GetSynonymGroup(synonymGroup.Id);
            var expectedResult = DatabaseContext.SynonymGroups.Find(synonymGroup.Id);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.SynonymWords);
                return options;
            });
        }

        [TestCase(TestName = "Given an synonymGroup id when the gateway is called with the id the gateway will delete the synonymGroup that matches")]
        public void GivenAnIdAMatchingSynonymGroupGetsDeleted()
        {
            var synonymGroup = EntityHelpers.CreateSynonymGroup();
            DatabaseContext.Add(synonymGroup);
            DatabaseContext.SaveChanges();
            _classUnderTest.DeleteSynonymGroup(synonymGroup.Id);
            var expectedResult = DatabaseContext.SynonymGroups.Find(synonymGroup.Id);
            expectedResult.Should().BeNull();
        }

        [TestCase(TestName = "Given an synonymGroup object when the gateway is called with the object the gateway will update the synonymGroup that matches")]
        public void GivenAnSynonymGroupAMatchingSynonymGroupGetsUpdated()
        {
            var synonymGroup = EntityHelpers.CreateSynonymGroup();
            DatabaseContext.Add(synonymGroup);
            DatabaseContext.SaveChanges();
            var synonymGroupDomain = _mapper.ToDomain(synonymGroup);
            synonymGroupDomain.Name = Randomm.Text();
            _classUnderTest.PatchSynonymGroup(synonymGroupDomain);
            var expectedResult = DatabaseContext.SynonymGroups.Find(synonymGroup.Id);
            expectedResult.Should().BeEquivalentTo(synonymGroupDomain, options =>
            {
                options.Excluding(ex => ex.SynonymWords);
                return options;
            });
        }

        [TestCase(TestName = "Given search parameters, when the gateway is called with the gateway will return a synonymGroup that match")]
        public void GivenSearchParametersMatchingSynonymGroupsGetReturned()
        {
            var synonymGroup = EntityHelpers.CreateSynonymGroup();
            DatabaseContext.AddRange(synonymGroup);
            DatabaseContext.SaveChanges();
            var searchParams = new SynonymGroupSearchRequest();
            var sgToFind = synonymGroup;
            searchParams.Search = sgToFind.Name;
            searchParams.Sort = "Name";
            searchParams.Direction = SortDirection.Asc.ToString();
            var gatewayResult = _classUnderTest.SearchSynonymGroups(searchParams);
            gatewayResult.Should().NotBeNull();
            gatewayResult.First().Should().BeEquivalentTo(sgToFind, options =>
            {
                options.Excluding(ex => ex.SynonymWords);
                return options;
            });
        }
    }
}
