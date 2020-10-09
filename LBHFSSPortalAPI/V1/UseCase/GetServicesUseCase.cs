using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetServicesUseCase : IGetServicesUseCase
    {
        private readonly IServicesGateway _servicesGateway;

        public GetServicesUseCase(IServicesGateway servicesGateway)
        {
            _servicesGateway = servicesGateway;
        }

        public async Task<ServiceResponse> Execute(int serviceId)
        {
            var serviceDomain = await _servicesGateway.GetServiceAsync(serviceId).ConfigureAwait(false);

            if (serviceDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not retrieve the service with an ID of '{serviceId}'",
                };

            return serviceDomain.ToResponse();
        }

        public async Task<List<ServiceResponse>> Execute(ServicesQueryParam servicesQuery)
        {
            var servicesDomain = await _servicesGateway.GetServicesAsync(servicesQuery).ConfigureAwait(false);

            if (servicesDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not retrieve services using the supplied search parameters"
                };

            return servicesDomain.ToResponse();
        }
    }
}
