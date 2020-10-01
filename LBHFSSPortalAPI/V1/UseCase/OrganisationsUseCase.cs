using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class OrganisationsUseCase : IOrganisationsUseCase
    {
        private readonly IOrganisationsGateway _organisationsGateway;

        public OrganisationsUseCase(IOrganisationsGateway organisationsGateway)
        {
            _organisationsGateway = organisationsGateway;
        }
        public OrganisationResponse ExecuteCreate(OrganisationRequest requestParams)
        {
            var gatewayResponse = _organisationsGateway.CreateOrganisation(requestParams.ToEntity());
            return gatewayResponse == null ? new OrganisationResponse() : gatewayResponse.ToResponse();
        }

        public OrganisationResponse ExecuteGet(int id)
        {
            var gatewayResponse = _organisationsGateway.GetOrganisation(id);
            return gatewayResponse == null ? new OrganisationResponse() : gatewayResponse.ToResponse();
        }

        public void ExecuteDelete(int id)
        {
            _organisationsGateway.DeleteOrganisation(id);
        }
    }
}
