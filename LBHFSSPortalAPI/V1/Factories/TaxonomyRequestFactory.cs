using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class TaxonomyRequestFactory
    {
        public static TaxonomyDomain ToDomain(this TaxonomyRequest request)
        {
            return new TaxonomyDomain()
            {
                Description = request.Description,
                Vocabulary = request.VocabularyId == 1 ? "category" : "demographic",
                Weight = request.Weight
            };
        }

        public static Taxonomy ToEntity(this TaxonomyRequest request)
        {
            return new Taxonomy()
            {
                Description = request.Description,
                Weight = request.Weight,
                Vocabulary = request.VocabularyId == 1 ? "category" : "demographic"
            };
        }
    }
}
