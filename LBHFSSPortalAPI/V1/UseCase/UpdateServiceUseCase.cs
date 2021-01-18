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

        private readonly IAddressXRefGateway _addressXRefGateway;

        public UpdateServiceUseCase(IServicesGateway servicesGateway, IAddressXRefGateway addressXRefGateway)
        {
            _servicesGateway = servicesGateway;
            _addressXRefGateway = addressXRefGateway;
        }

        public async Task<ServiceResponse> Execute(ServiceRequest request, int serviceId)
        {
            if ((request.Locations != null) && (request.Locations.Count > 0))
            {
                request.Locations.ForEach(l => l.NHSNeighbourhood = _addressXRefGateway.GetNHSNeighbourhood(l.Uprn));
            }

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
