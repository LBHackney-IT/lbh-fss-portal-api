using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class RequestFactory
    {
        public static OrganizationDomain ToDomain(this OrganisationRequest request)
        {
            return new OrganizationDomain()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                SubmittedAt = request.SubmittedAt,
                ReviewedAt = request.ReviewedAt,
                ReviewerMessage = request.ReviewerMessage,
                Status = request.Status,
                IsRegisteredCharity = request.IsRegisteredCharity,
                CharityNumber = request.CharityNumber,
                HasHcOrColGrant = request.HasHcOrColGrant,
                HasHcvsOrHgOrAelGrant = request.HasHcvsOrHgOrAelGrant,
                IsTraRegistered = request.IsTraRegistered,
                RslOrHaAssociation = request.RslOrHaAssociation,
                IsLotteryFunded = request.IsLotteryFunded,
                LotteryFundedProject = request.LotteryFundedProject,
                FundingOther = request.FundingOther,
                HasChildSupport = request.HasChildSupport,
                ChildSafeguardingLeadFirstName = request.ChildSafeguardingLeadFirstName,
                ChildSafeguardingLeadLastName = request.ChildSafeguardingLeadLastName,
                ChildSafeguardingLeadTrainingMonth = request.ChildSafeguardingLeadTrainingMonth,
                ChildSafeguardingLeadTrainingYear = request.ChildSafeguardingLeadTrainingYear,
                HasAdultSupport = request.HasAdultSupport,
                HasAdultSafeguardingLead = request.HasAdultSafeguardingLead,
                AdultSafeguardingLeadFirstName = request.AdultSafeguardingLeadFirstName,
                AdultSafeguardingLeadLastName = request.AdultSafeguardingLeadLastName,
                AdultSafeguardingLeadTrainingMonth = request.AdultSafeguardingLeadTrainingMonth,
                AdultSafeguardingLeadTrainingYear = request.AdultSafeguardingLeadTrainingYear,
                HasEnhancedSupport = request.HasEnhancedSupport,
                IsLocalOfferListed = request.IsLocalOfferListed,
            };
        }

        public static Organization ToEntity(this OrganisationRequest request)
        {
            return new Organization()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                SubmittedAt = request.SubmittedAt,
                ReviewedAt = request.ReviewedAt,
                ReviewerMessage = request.ReviewerMessage,
                Status = request.Status,
                IsRegisteredCharity = request.IsRegisteredCharity,
                CharityNumber = request.CharityNumber,
                HasHcOrColGrant = request.HasHcOrColGrant,
                HasHcvsOrHgOrAelGrant = request.HasHcvsOrHgOrAelGrant,
                IsTraRegistered = request.IsTraRegistered,
                RslOrHaAssociation = request.RslOrHaAssociation,
                IsLotteryFunded = request.IsLotteryFunded,
                LotteryFundedProject = request.LotteryFundedProject,
                FundingOther = request.FundingOther,
                HasChildSupport = request.HasChildSupport,
                ChildSafeguardingLeadFirstName = request.ChildSafeguardingLeadFirstName,
                ChildSafeguardingLeadLastName = request.ChildSafeguardingLeadLastName,
                ChildSafeguardingLeadTrainingMonth = request.ChildSafeguardingLeadTrainingMonth,
                ChildSafeguardingLeadTrainingYear = request.ChildSafeguardingLeadTrainingYear,
                HasAdultSupport = request.HasAdultSupport,
                HasAdultSafeguardingLead = request.HasAdultSafeguardingLead,
                AdultSafeguardingLeadFirstName = request.AdultSafeguardingLeadFirstName,
                AdultSafeguardingLeadLastName = request.AdultSafeguardingLeadLastName,
                AdultSafeguardingLeadTrainingMonth = request.AdultSafeguardingLeadTrainingMonth,
                AdultSafeguardingLeadTrainingYear = request.AdultSafeguardingLeadTrainingYear,
                HasEnhancedSupport = request.HasEnhancedSupport,
                IsLocalOfferListed = request.IsLocalOfferListed
            };
        }
    }
}
