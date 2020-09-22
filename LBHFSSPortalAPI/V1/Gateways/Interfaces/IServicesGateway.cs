using LBHFSSPortalAPI.V1.Domain;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IServicesGateway
    {
        Task<ServiceDomain> GetServiceAsync(int serviceId);
    }
}
