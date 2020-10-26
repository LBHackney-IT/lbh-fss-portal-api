using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IServicesGateway
    {
        Task<ServiceDomain> GetServiceAsync(int serviceId);
        Task<List<ServiceDomain>> GetServicesAsync(ServicesQueryParam servicesQuery);
        Task<ServiceDomain> CreateService(ServiceRequest request);
        Task DeleteService(int serviceId);
        Task DeleteUserService(int serviceId, int userId);
        Task<ServiceDomain> UpdateService(ServiceRequest request, int serviceId);
        void AddFileInfo(int serviceId, File fileEntity);
    }
}
