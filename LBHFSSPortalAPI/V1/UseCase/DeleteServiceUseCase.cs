using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class DeleteServiceUseCase : IDeleteServiceUseCase
    {
        private readonly IServicesGateway _servicesGateway;
        private readonly IUsersGateway _usersGateway;

        public DeleteServiceUseCase(IServicesGateway servicesGateway, IUsersGateway usersGateway)
        {
            _servicesGateway = servicesGateway;
            _usersGateway = usersGateway;
        }

        public async Task Execute(int serviceId, UserClaims userClaims)
        {
            if (userClaims.UserRole == "Admin")
            {
                LambdaLogger.Log($"User role is admin, delete serviceId {serviceId}");
                await _servicesGateway.DeleteService(serviceId).ConfigureAwait(false);
            }
            else
            {
                var service = await _servicesGateway.GetServiceAsync(serviceId).ConfigureAwait(false);
                var user = await _usersGateway.GetUserByIdAsync(userClaims.UserId).ConfigureAwait(false);
                var orgs = user.UserOrganisations.Where(x => x.OrganisationId == service.OrganisationId).ToList();
                if (orgs.Count == 0)
                {
                    LambdaLogger.Log($"UserId {userClaims.UserId} is not in Organisation {service.OrganisationId} for serviceId {serviceId}");
                    throw new UseCaseException
                    {
                        UserErrorMessage = $"Could not delete service with an ID of '{serviceId}'",
                    };
                }
                else
                {
                    LambdaLogger.Log($"UserId {userClaims.UserId} is in Organisation {service.OrganisationId} for serviceId {serviceId} so service can be deleted");
                    await _servicesGateway.DeleteService(serviceId).ConfigureAwait(false);
                }
            }
        }
    }
}
