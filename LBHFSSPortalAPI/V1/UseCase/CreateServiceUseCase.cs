using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class CreateServiceUseCase : ICreateServiceUseCase
    {
        private readonly IServicesGateway _servicesGateway;
        private readonly IAddressXRefGateway _addressXRefGateway;

        public CreateServiceUseCase(IServicesGateway servicesGateway, IAddressXRefGateway addressXRefGateway)
        {
            _servicesGateway = servicesGateway;
            _addressXRefGateway = addressXRefGateway;
        }

        public async Task<ServiceResponse> Execute(ServiceRequest request)
        {
            if ((request.Locations != null) && (request.Locations.Count > 0))
            {
                request.Locations.ForEach(l => l.NHSNeighbourhood = _addressXRefGateway.GetNHSNeighbourhood(l.Uprn));
            }

            var serviceDomain = await _servicesGateway.CreateService(request).ConfigureAwait(false);

            if (serviceDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not create the new service.",
                };

            return serviceDomain.ToResponse();
        }
    }
}
