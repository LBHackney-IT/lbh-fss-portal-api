using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.Validations
{
    public static class TaxonomyValidators
    {
        public static bool IsValid(this TaxonomyRequest createRequest)
        {
            return (createRequest.VocabularyId == 1 || createRequest.VocabularyId == 2);
        }
    }
}
