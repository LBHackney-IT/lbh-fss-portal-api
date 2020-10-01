using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IOrganisationsGateway
    {
        OrganizationDomain CreateOrganisation(Organization request);
    }
}
