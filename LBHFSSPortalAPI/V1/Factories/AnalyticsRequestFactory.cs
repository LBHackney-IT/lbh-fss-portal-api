using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class AnalyticsRequestFactory
    {
        public static AnalyticsEventQuery ToQuery(this AnalyticsRequest request)
        {
            return new AnalyticsEventQuery()
            {
                OrganisationId = request.OrganisationId,
                StartDateTime = request.StartDate,
                EndDateTime = request.EndDate
            };
        }
    }
}
