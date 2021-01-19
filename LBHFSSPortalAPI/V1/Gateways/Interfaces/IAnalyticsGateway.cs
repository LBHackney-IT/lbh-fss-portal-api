using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IAnalyticsGateway
    {
        List<AnalyticsEventDomain> GetAnalyticsEvents(AnalyticsEventQuery analyticsEventQuery);
    }
}
