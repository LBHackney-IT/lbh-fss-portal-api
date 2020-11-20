using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Enums;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class UserOrganisationLinksUseCase : IUserOrganisationLinksUseCase
    {
        private readonly IUserOrganisationLinksGateway _userOrganisationLinksGateway;

        public UserOrganisationLinksUseCase(IUserOrganisationLinksGateway userOrganisationLinksGateway)
        {
            _userOrganisationLinksGateway = userOrganisationLinksGateway;
        }

        public UserOrganisationLinkResponse ExecuteCreate(UserOrganisationLinkRequest requestParams)
        {
            var gatewayResponse = _userOrganisationLinksGateway.LinkUserToOrganisation(requestParams.OrganisationId, requestParams.UserId);
            return gatewayResponse == null ? new UserOrganisationLinkResponse() : gatewayResponse.ToResponse();
        }

        public void ExecuteDelete(int userId)
        {
            _userOrganisationLinksGateway.DeleteUserOrganisationLink(userId);
        }
    }
}
