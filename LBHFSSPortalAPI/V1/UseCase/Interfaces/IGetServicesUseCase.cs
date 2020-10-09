using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IGetServicesUseCase
    {
        Task<ServiceResponse> Execute(int serviceId);
        Task<List<ServiceResponse>> Execute(ServicesQueryParam servicesQuery);
    }
}
