using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IServicesGateway
    {
        Task<ServiceDomain> GetServiceAsync(int serviceId);
        Task<List<ServiceDomain>> GetServicesAsync(ServicesQueryParam servicesQuery);
        Task<ServiceDomain> CreateService(ServiceRequest request);
        Task DeleteService(int serviceId);
        Task<ServiceDomain> UpdateService(ServiceRequest request, int serviceId);
    }
}
