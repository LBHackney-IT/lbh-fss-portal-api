using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Enums;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;

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
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public TaxonomyResponse ExecuteCreate(TaxonomyRequest requestParams)
        {
            if (!requestParams.IsValid())
                throw new InvalidOperationException("vocabulary_id must be provided and can only be 1 or 2.");
            var gatewayResponse = _taxonomyGateway.CreateTaxonomy(requestParams.ToEntity());
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public void ExecuteDelete(int id)
        {
            LambdaLogger.Log($"Delete taxonomy {id}");
            _taxonomyGateway.DeleteTaxonomy(id);
        }

        public TaxonomyResponse ExecutePatch(int id, TaxonomyRequest requestParams)
        {
            if (!requestParams.IsValid())
                throw new InvalidOperationException("vocabulary_id must be provided and can only be 1 or 2.");
            var gatewayResponse = _taxonomyGateway.PatchTaxonomy(id, requestParams.ToEntity());
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public TaxonomyResponseList ExecuteGet(int? vocabularyId)
        {
            List<TaxonomyDomain> gatewayResponse;
            if (vocabularyId != null)
            {
                string vocabulary = vocabularyId == 1 ? "category" : "demographic";
                gatewayResponse = _taxonomyGateway.GetTaxonomiesByVocabulary(vocabulary);
            }
            else
            {
                gatewayResponse = _taxonomyGateway.GetAllTaxonomies();
            }
            return new TaxonomyResponseList
            {
                Categories = gatewayResponse?.Where(x => x.Vocabulary == "category").ToList().ToResponse(),
                Demographics = gatewayResponse?.Where(x => x.Vocabulary == "demographic").ToList().ToResponse()
            };
        }
    }
}
