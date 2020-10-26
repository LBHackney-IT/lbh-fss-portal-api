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
            //user is admin, delete away
            if (userClaims.UserRole == "Admin")
                await _servicesGateway.DeleteService(serviceId).ConfigureAwait(false);
            else
            {
                var service = await _servicesGateway.GetServiceAsync(serviceId).ConfigureAwait(false);
                //user is not admin - check they are in the org of the service being deleted
                var user = await _usersGateway.GetUserByIdAsync(userClaims.UserId).ConfigureAwait(false);
                var orgs = user.Organisations.Where(x => x.Id == service.OrganisationId).ToList();
                if (orgs.Count == 0)
                    throw new UseCaseException
                    {
                        UserErrorMessage = $"Could not delete service with an ID of '{serviceId}'",
                    };
                else
                {
                    await _servicesGateway.DeleteService(serviceId).ConfigureAwait(false);
                }
            }
        }
    }
}
