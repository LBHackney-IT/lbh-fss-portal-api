using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class AnalyticsResponseFactory
    {
        public static AnalyticsResponse ToResponse(this AnalyticsEventDomain domain)
        {
            var response = domain == null
                ? null
                : new AnalyticsResponse
                {
                    Id = domain.Id,
                    ServiceName = domain.Service.Name,
                    TimeStamp = domain.TimeStamp
                };

            return response;
        }

        public static AnalyticsResponseList ToResponse(this List<AnalyticsEventDomain> analyticsEvents)
        {
            return new AnalyticsResponseList { AnalyticEvents = analyticsEvents.Select(o => o.ToResponse()).ToList() };
        }
    }
}
