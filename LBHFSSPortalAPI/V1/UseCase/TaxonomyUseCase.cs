using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Enums;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class TaxonomyUseCase : ITaxonomyUseCase
    {
        private readonly ITaxonomyGateway _taxonomyGateway;

        public TaxonomyUseCase(ITaxonomyGateway taxonomyGateway)
        {
            _taxonomyGateway = taxonomyGateway;
        }

        public TaxonomyResponse ExecuteGetById(int id)
        {
            var gatewayResponse = _taxonomyGateway.GetTaxonomy(id);
            return gatewayResponse == null ? new TaxonomyResponse() : gatewayResponse.ToResponse();
        }

        public TaxonomyResponse ExecuteCreate(TaxonomyRequest requestParams)
        {
            var gatewayResponse = _taxonomyGateway.CreateTaxonomy(requestParams.ToEntity());
            return gatewayResponse == null ? new TaxonomyResponse() : gatewayResponse.ToResponse();
        }

        public List<TaxonomyResponse> ExecuteGetAll()
        {
            var gatewayResponse = _taxonomyGateway.GetAllTaxonomies();
            return gatewayResponse == null ? new List<TaxonomyResponse>() : gatewayResponse.ToResponse().ToList();
        }

        public List<TaxonomyResponse> ExecuteGetByVocabulary(int vocabularyId)
        {
            string vocabulary = vocabularyId == 1 ? "category" : "demographic";
            var gatewayResponse = _taxonomyGateway.GetTaxonomiesByVocabulary(vocabulary);
            return gatewayResponse == null ? new List<TaxonomyResponse>() : gatewayResponse.ToResponse().ToList();
        }

        public void ExecuteDelete(int id)
        {
            LambdaLogger.Log($"Delete taxonomy {id}");
            _taxonomyGateway.DeleteTaxonomy(id);
        }

        public TaxonomyResponse ExecutePatch(int id, TaxonomyRequest requestParams)
        {
            var gatewayResponse = _taxonomyGateway.PatchTaxonomy(id, requestParams.ToEntity());
            return gatewayResponse == null ? new TaxonomyResponse() : gatewayResponse.ToResponse();
        }
    }
}
