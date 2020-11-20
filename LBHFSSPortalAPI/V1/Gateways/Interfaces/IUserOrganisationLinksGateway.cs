using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IUserOrganisationLinksGateway
    {
        void DeleteUserOrganisationLink(int userId);
        UserOrganisationDomain LinkUserToOrganisation(int organisationId, int userId);
    }
}
