using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface ITaxonomyGateway
    {
        TaxonomyDomain CreateTaxonomy(Taxonomy taxonomy);
        TaxonomyDomain GetTaxonomy(int id);
        void DeleteTaxonomy(int id);
        TaxonomyDomain PatchTaxonomy(int id, Taxonomy taxonomy);
        List<TaxonomyDomain> GetAllTaxonomies();
        List<TaxonomyDomain> GetTaxonomiesByVocabulary(string vocabulary);
        List<ServiceTaxonomyDomain> GetServiceTaxonomies(int taxonomyId);
    }
}
