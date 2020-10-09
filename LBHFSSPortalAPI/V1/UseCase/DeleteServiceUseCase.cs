using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class DeleteServiceUseCase : IDeleteServiceUseCase
    {
        private readonly IServicesGateway _servicesGateway;

        public DeleteServiceUseCase(IServicesGateway servicesGateway)
        {
            _servicesGateway = servicesGateway;
        }

        public async Task Execute(int serviceId)
        {
            await _servicesGateway.DeleteService(serviceId).ConfigureAwait(false);
        }
    }
}
