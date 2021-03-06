using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IAnalyticsUseCase
    {
        AnalyticsResponseList ExecuteGet(AnalyticsRequest requestParams);
    }
}
