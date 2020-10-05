using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class CreateServiceUseCase : ICreateServiceUseCase
    {
        private readonly IServicesGateway _servicesGateway;

        public CreateServiceUseCase(IServicesGateway servicesGateway)
        {
            _servicesGateway = servicesGateway;
        }

        public async Task<ServiceResponse> Execute(ServiceRequest request)
        {
            Validate(request);
            var serviceDomain = await _servicesGateway.CreateService(request).ConfigureAwait(false);

            return serviceDomain.ToResponse();
        }

        private void Validate(ServiceRequest request)
        {
            // all fine for now :)
        }
    }
}
