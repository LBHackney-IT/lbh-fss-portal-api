using System.Threading.Tasks;
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

        public async Task<OrganisationResponseList> ExecuteGet(OrganisationSearchRequest requestParams)
        {
            var gatewayResponse = await _organisationsGateway.SearchOrganisations(requestParams).ConfigureAwait(false);
            return gatewayResponse == null ? new OrganisationResponseList() : gatewayResponse.ToResponse();
        }

        public OrganisationResponse ExecutePatch(int id, OrganisationRequest request)
        {
            var organisationDomain = _organisationsGateway.GetOrganisation(id);
            if (organisationDomain == null)
                return null;
            organisationDomain.Name = request.Name ?? organisationDomain.Name;
            organisationDomain.CreatedAt = request.CreatedAt ?? organisationDomain.CreatedAt;
            organisationDomain.Status = request.Status ?? organisationDomain.Status;
            organisationDomain.UpdatedAt = request.UpdatedAt;
            organisationDomain.SubmittedAt = request.SubmittedAt;
            organisationDomain.ReviewedAt = request.ReviewedAt;
            organisationDomain.ReviewerMessage = request.ReviewerMessage;
            organisationDomain.Status = request.Status;
            organisationDomain.IsRegisteredCharity = request.IsRegisteredCharity;
            organisationDomain.CharityNumber = request.CharityNumber;
            organisationDomain.HasHcOrColGrant = request.HasHcOrColGrant;
            organisationDomain.HasHcvsOrHgOrAelGrant = request.HasHcvsOrHgOrAelGrant;
            organisationDomain.IsTraRegistered = request.IsTraRegistered;
            organisationDomain.RslOrHaAssociation = request.RslOrHaAssociation;
            organisationDomain.IsLotteryFunded = request.IsLotteryFunded;
            organisationDomain.LotteryFundedProject = request.LotteryFundedProject;
            organisationDomain.FundingOther = request.FundingOther;
            organisationDomain.HasChildSupport = request.HasChildSupport;
            organisationDomain.ChildSafeguardingLeadFirstName = request.ChildSafeguardingLeadFirstName;
            organisationDomain.ChildSafeguardingLeadLastName = request.ChildSafeguardingLeadLastName;
            organisationDomain.ChildSafeguardingLeadTrainingMonth = request.ChildSafeguardingLeadTrainingMonth;
            organisationDomain.ChildSafeguardingLeadTrainingYear = request.ChildSafeguardingLeadTrainingYear;
            organisationDomain.HasAdultSupport = request.HasAdultSupport;
            organisationDomain.HasAdultSafeguardingLead = request.HasAdultSafeguardingLead;
            organisationDomain.AdultSafeguardingLeadFirstName = request.AdultSafeguardingLeadFirstName;
            organisationDomain.AdultSafeguardingLeadLastName = request.AdultSafeguardingLeadLastName;
            organisationDomain.AdultSafeguardingLeadTrainingMonth = request.AdultSafeguardingLeadTrainingMonth;
            organisationDomain.AdultSafeguardingLeadTrainingYear = request.AdultSafeguardingLeadTrainingYear;
            organisationDomain.HasEnhancedSupport = request.HasEnhancedSupport;
            organisationDomain.IsLocalOfferListed = request.IsLocalOfferListed;
            var gatewayResponse = _organisationsGateway.PatchOrganisation(organisationDomain);
            return gatewayResponse.ToResponse();
        }


        public void ExecuteDelete(int id)
        {
            _organisationsGateway.DeleteOrganisation(id);
        }
    }
}
