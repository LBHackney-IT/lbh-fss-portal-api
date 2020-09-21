using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetServicesUseCase : IGetServicesUseCase
    {
        public Task<ServiceResponse> Execute(int serviceId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ServiceResponse>> Execute(ServicesQueryParam servicesQuery)
        {
            throw new System.NotImplementedException();
        }
    }
}
