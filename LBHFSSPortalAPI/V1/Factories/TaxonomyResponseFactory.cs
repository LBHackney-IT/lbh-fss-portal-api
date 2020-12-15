using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class TaxonomyResponseFactory
    {
        public static TaxonomyResponse ToResponse(this TaxonomyDomain domain)
        {
            var response = domain == null
                ? null
                : new TaxonomyResponse
                {
                    Id = domain.Id,
                    Name = domain.Name,
                    Description = domain.Description,
                    Weight = domain.Weight,
                    Vocabulary = domain.Vocabulary,
                    VocabularyId = domain.Vocabulary == "category" ? 1 : 2
                };

            return response;
        }

        public static List<TaxonomyResponse> ToResponse(this List<TaxonomyDomain> taxonomies)
        {
            return taxonomies.Select(s => ToResponse(s)).ToList();
        }
    }
}
