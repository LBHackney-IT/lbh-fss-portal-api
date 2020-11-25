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
    public class UserOrganisationLinksUseCase : IUserOrganisationUseCase
    {
        private readonly IUserOrganisationGateway _userOrganisationLinksGateway;

        public UserOrganisationLinksUseCase(IUserOrganisationGateway userOrganisationLinksGateway)
        {
            _userOrganisationLinksGateway = userOrganisationLinksGateway;
        }

        public UserOrganisationResponse ExecuteCreate(UserOrganisationRequest requestParams)
        {
            var checkAlreadyExists = _userOrganisationLinksGateway.GetUserOrganisationByUserAndOrgId(requestParams.UserId, requestParams.OrganisationId);
            if (checkAlreadyExists != null)
            {
                throw new UseCaseException()
                {
                    DevErrorMessage = "UserOrganisation link already exists for provided User and Organisation",
                    UserErrorMessage = "Provided user is already linked to that organisation"
                };
            }

            var gatewayResponse = _userOrganisationLinksGateway.LinkUserToOrganisation(requestParams.OrganisationId, requestParams.UserId);
            return gatewayResponse == null ? new UserOrganisationResponse() : gatewayResponse.ToResponse();
        }

        public void ExecuteDelete(int userId)
        {
            _userOrganisationLinksGateway.DeleteUserOrganisationLink(userId);
        }
    }
}
