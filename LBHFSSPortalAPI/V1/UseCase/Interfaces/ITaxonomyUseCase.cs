using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface ITaxonomyUseCase
    {
        TaxonomyResponse ExecuteCreate(TaxonomyRequest requestParams);
        TaxonomyResponse ExecuteGetById(int id);
        List<TaxonomyResponse> ExecuteGetAll();
        List<TaxonomyResponse> ExecuteGetByVocabulary(int vocabularyId);
        TaxonomyResponse ExecutePatch(int id, TaxonomyRequest requestParams);
        void ExecuteDelete(int id);
    }
}
