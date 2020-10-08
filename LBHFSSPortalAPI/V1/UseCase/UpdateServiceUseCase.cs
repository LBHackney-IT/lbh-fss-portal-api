using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class UpdateServiceUseCase : IUpdateServiceUseCase
    {
        private readonly IServicesGateway _servicesGateway;

        public UpdateServiceUseCase(IServicesGateway servicesGateway)
        {
            _servicesGateway = servicesGateway;
        }

        public async Task<ServiceResponse> Execute(ServiceRequest request, int serviceId)
        {
            var serviceDomain = await _servicesGateway.UpdateService(request, serviceId).ConfigureAwait(false);

            if (serviceDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not update the service with ID '{serviceId}'",
                };

            return serviceDomain.ToResponse();
        }
    }
}
