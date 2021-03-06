using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class RequestFactory
    {
        public static OrganisationDomain ToDomain(this OrganisationRequest request)
        {
            return new OrganisationDomain()
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
                IsRegisteredCommunityInterestCompany = request.IsRegisteredCommunityInterestCompany,
                CommunityInterestCompanyNumber = request.CommunityInterestCompanyNumber,
                HasHcOrColGrant = request.HasHcOrColGrant,
                HasHcvsOrHgOrAelGrant = request.HasHcvsOrHgOrAelGrant,
                IsTraRegistered = request.IsTraRegistered,
                IsHackneyBased = request.IsHackneyBased,
                RslOrHaAssociation = request.RslOrHaAssociation,
                IsLotteryFunded = request.IsLotteryFunded,
                LotteryFundedProject = request.LotteryFundedProject,
                FundingOther = request.FundingOther,
                HasChildSupport = request.HasChildSupport,
                HasChildSafeguardingLead = request.HasChildSafeguardingLead,
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
                ReviewerUid = request.ReviewerId
            };
        }

        public static Organisation ToEntity(this OrganisationRequest request)
        {
            return new Organisation()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                SubmittedAt = request.SubmittedAt,
                ReviewedAt = request.ReviewedAt,
                ReviewerMessage = request.ReviewerMessage,
                Status = request.Status,
                IsRegisteredCharity = request.IsRegisteredCharity,
                IsHackneyBased = request.IsHackneyBased,
                CharityNumber = request.CharityNumber,
                IsRegisteredCommunityInterestCompany = request.IsRegisteredCommunityInterestCompany,
                CommunityInterestCompanyNumber = request.CommunityInterestCompanyNumber,
                HasHcOrColGrant = request.HasHcOrColGrant,
                HasHcvsOrHgOrAelGrant = request.HasHcvsOrHgOrAelGrant,
                IsTraRegistered = request.IsTraRegistered,
                RslOrHaAssociation = request.RslOrHaAssociation,
                IsLotteryFunded = request.IsLotteryFunded,
                LotteryFundedProject = request.LotteryFundedProject,
                FundingOther = request.FundingOther,
                HasChildSupport = request.HasChildSupport,
                HasChildSafeguardingLead = request.HasChildSafeguardingLead,
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
                ReviewerUid = request.ReviewerId
            };
        }

        public static SynonymGroupDomain ToDomain(this SynonymGroupRequest request)
        {
            return new SynonymGroupDomain()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt
            };
        }

        public static SynonymWordDomain ToDomain(this SynonymWordRequest request)
        {
            return new SynonymWordDomain()
            {
                Word = request.Word,
                GroupId = request.GroupId,
                CreatedAt = request.CreatedAt
            };
        }

        public static SynonymGroup ToEntity(this SynonymGroupRequest request)
        {
            return new SynonymGroup()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt
            };
        }

        public static SynonymWord ToEntity(this SynonymWordRequest request)
        {
            return new SynonymWord()
            {
                Word = request.Word,
                GroupId = request.GroupId,
                CreatedAt = request.CreatedAt
            };
        }
    }
}
