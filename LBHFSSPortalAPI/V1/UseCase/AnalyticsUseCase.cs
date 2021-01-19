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

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class AnalyticsUseCase : IAnalyticsUseCase
    {
        private readonly IAnalyticsGateway _analyticsGateway;

        public AnalyticsUseCase(IAnalyticsGateway analyticsGateway)
        {
            _analyticsGateway = analyticsGateway;
        }

        public AnalyticsResponseList ExecuteGet(AnalyticsRequest requestParams)
        {
            var gatewayResponse = _analyticsGateway.GetAnalyticsEvents(requestParams.ToQuery());
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }
    }
}
