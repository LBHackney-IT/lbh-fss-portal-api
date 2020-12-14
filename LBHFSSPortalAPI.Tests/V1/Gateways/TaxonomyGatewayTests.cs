using System;
using System.Linq;
using AutoMapper;
using Bogus;
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
    [TestFixture]
    public class TaxonomyGatewayTests : DatabaseTests
    {
        private TaxonomyGateway _classUnderTest;
        private MappingHelper _mapper = new MappingHelper();

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new TaxonomyGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given a valid taxonomy when the gateway is called the gateway will create the taxonomy")]
        public void GivenTaxonomyDomainTaxonomyGetsCreated()
        {
            var taxonomy = EntityHelpers.CreateTaxonomy();
            var gatewayResult = _classUnderTest.CreateTaxonomy(taxonomy);
            var expectedResult = DatabaseContext.Taxonomies.Where(x => x.Name == taxonomy.Name).FirstOrDefault();
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(_mapper.ToDomain(expectedResult), options =>
            {
                return options;
            });
        }

        [TestCase(TestName = "Given a taxonomy id when the gateway is called with the id the gateway will return an taxonomy that matches")]
        public void GivenAnIdAMatchingTaxonomyGetsReturned()
        {
            var taxonomy = EntityHelpers.CreateTaxonomy();
            DatabaseContext.Add(taxonomy);
            DatabaseContext.SaveChanges();
            var gatewayResult = _classUnderTest.GetTaxonomy(taxonomy.Id);
            var expectedResult = DatabaseContext.Taxonomies.Find(taxonomy.Id);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(expectedResult, options =>
            {
                options.Excluding(ex => ex.ServiceTaxonomies);
                return options;
            });
        }

        [TestCase(TestName = "Given an taxonomy id when the gateway is called with the id the gateway will delete the taxonomy that matches")]
        public void GivenAnIdAMatchingTaxonomyGetsDeleted()
        {
            var taxonomy = EntityHelpers.CreateTaxonomy();
            DatabaseContext.Add(taxonomy);
            DatabaseContext.SaveChanges();
            _classUnderTest.DeleteTaxonomy(taxonomy.Id);
            var expectedResult = DatabaseContext.Taxonomies.Find(taxonomy.Id);
            expectedResult.Should().BeNull();
        }

        [TestCase(TestName = "Given an taxonomy id when the gateway is called with the id the gateway will delete the taxonomy that matches AND the matching service taxonomy records")]
        public void GivenAnIdAMatchingTaxonomyAndServiceTaxonomyGetsDeleted()
        {
            var serviceTaxonomy = EntityHelpers.CreateServiceTaxonomy();
            var taxonomy = EntityHelpers.CreateTaxonomy();
            serviceTaxonomy.Taxonomy = taxonomy;
            DatabaseContext.Add(taxonomy);
            DatabaseContext.Add(serviceTaxonomy);
            DatabaseContext.SaveChanges();
            _classUnderTest.DeleteTaxonomy(taxonomy.Id);
            var expectedResult = DatabaseContext.Taxonomies.Find(serviceTaxonomy.Id);
            expectedResult.Should().BeNull();
            var expectedResult2 = DatabaseContext.ServiceTaxonomies.Where(x => x.TaxonomyId == taxonomy.Id);
            expectedResult2.Should().BeEmpty();
        }

        [TestCase(TestName = "Given a taxonomy when the gateway is called with an existing taxonomy the gateway will update the taxonomy that matches")]
        public void GivenATaxonomyTaxonomyGetsUpdated()
        {
            var taxonomy = EntityHelpers.CreateTaxonomy();
            DatabaseContext.Add(taxonomy);
            DatabaseContext.SaveChanges();
            var newTaxonomy = taxonomy;
            newTaxonomy.Description = Randomm.Text();
            _classUnderTest.PatchTaxonomy(newTaxonomy);
            var gatewayResult = DatabaseContext.Taxonomies.Find(taxonomy.Id);
            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeEquivalentTo(_mapper.ToDomain(newTaxonomy), options =>
            {
                return options;
            });
        }
    }
}
