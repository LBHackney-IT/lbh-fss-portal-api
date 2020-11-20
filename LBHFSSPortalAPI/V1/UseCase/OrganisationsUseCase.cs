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
    public class OrganisationsUseCase : IOrganisationsUseCase
    {
        private readonly IOrganisationsGateway _organisationsGateway;
        private readonly INotifyGateway _notifyGateway;
        private readonly IUsersGateway _usersGateway;
        private readonly ISessionsGateway _sessionsGateway;

        public OrganisationsUseCase(IOrganisationsGateway organisationsGateway,
            INotifyGateway notifyGateway,
            IUsersGateway usersGateway,
            ISessionsGateway sessionsGateway)
        {
            _usersGateway = usersGateway;
            _organisationsGateway = organisationsGateway;
            _notifyGateway = notifyGateway;
            _sessionsGateway = sessionsGateway;
        }
        public OrganisationResponse ExecuteCreate(string accessToken, OrganisationRequest requestParams)
        {
            var gatewayResponse = _organisationsGateway.CreateOrganisation(requestParams.ToEntity());
            if (gatewayResponse != null)
            {
                var session = _sessionsGateway.GetSessionByToken(accessToken);
                if (session != null)
                    _organisationsGateway.LinkUserToOrganisation(gatewayResponse.ToEntity(), session.User);
                var userQueryParam = new UserQueryParam { Sort = "Name", Direction = "asc" };
                var adminUsers = _usersGateway.GetAllUsers(userQueryParam).Result
                    .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Admin"));
                var adminEmails = adminUsers.Select(au => au.Email).ToArray();
                _notifyGateway.SendMessage(NotifyMessageTypes.AdminNotification, adminEmails, requestParams.StatusMessage);
            }
            return gatewayResponse == null ? new OrganisationResponse() : gatewayResponse.ToResponse();
        }

        public OrganisationResponse ExecuteGet(int id)
        {
            var gatewayResponse = _organisationsGateway.GetOrganisation(id);
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
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
            organisationDomain.UpdatedAt = request.UpdatedAt ?? organisationDomain.UpdatedAt;
            organisationDomain.SubmittedAt = request.SubmittedAt ?? organisationDomain.SubmittedAt;
            organisationDomain.ReviewedAt = request.ReviewedAt ?? organisationDomain.ReviewedAt;
            organisationDomain.ReviewerMessage = request.ReviewerMessage ?? organisationDomain.ReviewerMessage;
            organisationDomain.Status = request.Status ?? organisationDomain.Status;
            organisationDomain.IsRegisteredCharity = request.IsRegisteredCharity ?? organisationDomain.IsRegisteredCharity;
            organisationDomain.IsHackneyBased = request.IsHackneyBased ?? organisationDomain.IsRegisteredCharity;
            organisationDomain.CharityNumber = request.CharityNumber ?? organisationDomain.CharityNumber;
            organisationDomain.HasHcOrColGrant = request.HasHcOrColGrant ?? organisationDomain.HasHcOrColGrant;
            organisationDomain.HasHcvsOrHgOrAelGrant = request.HasHcvsOrHgOrAelGrant ?? organisationDomain.HasHcvsOrHgOrAelGrant;
            organisationDomain.IsTraRegistered = request.IsTraRegistered ?? organisationDomain.IsTraRegistered;
            organisationDomain.RslOrHaAssociation = request.RslOrHaAssociation ?? organisationDomain.RslOrHaAssociation;
            organisationDomain.IsLotteryFunded = request.IsLotteryFunded ?? organisationDomain.IsLotteryFunded;
            organisationDomain.LotteryFundedProject = request.LotteryFundedProject ?? organisationDomain.LotteryFundedProject;
            organisationDomain.FundingOther = request.FundingOther ?? organisationDomain.FundingOther;
            organisationDomain.HasChildSupport = request.HasChildSupport ?? organisationDomain.HasChildSupport;
            organisationDomain.HasChildSafeguardingLead = request.HasChildSafeguardingLead ?? organisationDomain.HasChildSafeguardingLead;
            organisationDomain.ChildSafeguardingLeadFirstName = request.ChildSafeguardingLeadFirstName ?? organisationDomain.ChildSafeguardingLeadFirstName;
            organisationDomain.ChildSafeguardingLeadLastName = request.ChildSafeguardingLeadLastName ?? organisationDomain.ChildSafeguardingLeadLastName;
            organisationDomain.ChildSafeguardingLeadTrainingMonth = request.ChildSafeguardingLeadTrainingMonth ?? organisationDomain.ChildSafeguardingLeadTrainingMonth;
            organisationDomain.ChildSafeguardingLeadTrainingYear = request.ChildSafeguardingLeadTrainingYear ?? organisationDomain.ChildSafeguardingLeadTrainingYear;
            organisationDomain.HasAdultSupport = request.HasAdultSupport ?? organisationDomain.HasAdultSupport;
            organisationDomain.HasAdultSafeguardingLead = request.HasAdultSafeguardingLead ?? organisationDomain.HasAdultSafeguardingLead;
            organisationDomain.AdultSafeguardingLeadFirstName = request.AdultSafeguardingLeadFirstName ?? organisationDomain.AdultSafeguardingLeadFirstName;
            organisationDomain.AdultSafeguardingLeadLastName = request.AdultSafeguardingLeadLastName ?? organisationDomain.AdultSafeguardingLeadLastName;
            organisationDomain.AdultSafeguardingLeadTrainingMonth = request.AdultSafeguardingLeadTrainingMonth ?? organisationDomain.AdultSafeguardingLeadTrainingMonth;
            organisationDomain.AdultSafeguardingLeadTrainingYear = request.AdultSafeguardingLeadTrainingYear ?? organisationDomain.AdultSafeguardingLeadTrainingYear;
            organisationDomain.HasEnhancedSupport = request.HasEnhancedSupport ?? organisationDomain.HasEnhancedSupport;
            organisationDomain.IsLocalOfferListed = request.IsLocalOfferListed ?? organisationDomain.IsLocalOfferListed;
            organisationDomain.ReviewerUid = request.ReviewerId ?? organisationDomain.ReviewerUid;
            var gatewayResponse = _organisationsGateway.PatchOrganisation(organisationDomain);
            if (gatewayResponse != null && gatewayResponse.Status.ToLower() == "published")
            {
                var orgUserEmails = gatewayResponse.UserOrganisations
                    .Select(uo => uo.User.Email).ToArray();
                _notifyGateway.SendMessage(NotifyMessageTypes.StatusUpdate, orgUserEmails, request.StatusMessage);
            }
            if (gatewayResponse != null && gatewayResponse.Status.ToLower() == "rejected")
            {
                var orgUserEmails = gatewayResponse.UserOrganisations
                    .Select(uo => uo.User.Email).ToArray();
                _notifyGateway.SendMessage(NotifyMessageTypes.NotApproved, orgUserEmails, request.StatusMessage);
            }
            if (gatewayResponse != null && gatewayResponse.Status.ToLower() == "awaiting review")
            {
                var userQueryParam = new UserQueryParam { Sort = "Name", Direction = "asc" };
                var adminUsers = _usersGateway.GetAllUsers(userQueryParam).Result
                    .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Admin"));
                var adminEmails = adminUsers.Select(au => au.Email).ToArray();
                _notifyGateway.SendMessage(NotifyMessageTypes.AdminNotification, adminEmails, request.StatusMessage);
            }
            return gatewayResponse.ToResponse();
        }


        public void ExecuteDelete(int id, UserClaims userClaims)
        {
            if (userClaims.UserRole == "Admin")
            {
                LambdaLogger.Log($"User role is admin, delete orgId {id}");
                _organisationsGateway.DeleteOrganisation(id);
            }
            else
            {
                var org = _organisationsGateway.GetOrganisation(id);
                var orgs = org.UserOrganisations.Where(x => x.UserId == userClaims.UserId).ToList();
                if (orgs.Count == 0)
                {
                    LambdaLogger.Log($"UserId {userClaims.UserId} is not in Organisation {id}");
                    throw new UseCaseException
                    {
                        UserErrorMessage = $"Could not delete organization with an ID of '{id}'",
                    };
                }
                else
                {
                    LambdaLogger.Log($"UserId {userClaims.UserId} is in Organisation {id} so organisation can be deleted");
                    _organisationsGateway.DeleteOrganisation(id);
                }
            }
        }
    }
}
