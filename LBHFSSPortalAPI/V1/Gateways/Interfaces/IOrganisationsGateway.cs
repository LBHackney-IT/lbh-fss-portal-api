using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IOrganisationsGateway
    {
        OrganisationDomain CreateOrganisation(Organisation request);
        OrganisationDomain GetOrganisation(int id);
        Task<List<OrganisationDomain>> SearchOrganisations(OrganisationSearchRequest requestParams);
        void DeleteOrganisation(int id);
        OrganisationDomain PatchOrganisation(OrganisationDomain organisationDomain);
    }
}
