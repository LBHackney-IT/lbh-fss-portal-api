using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static User ToEntity(this UserDomain userDomain)
        {
            return new User
            {
                Id = userDomain.Id,
                Email = userDomain.Email,
                Name = userDomain.Name,
                Status = userDomain.Status,
                CreatedAt = userDomain.CreatedAt,
                SubId = userDomain.SubId
            };
        }

        public static Organisation ToEntity(this OrganisationDomain domain)
        {
            return new Organisation
            {
                Id = domain.Id,
                Name = domain.Name,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                SubmittedAt = domain.SubmittedAt,
                ReviewedAt = domain.ReviewedAt,
                ReviewerMessage = domain.ReviewerMessage,
                Status = domain.Status,
                IsRegisteredCharity = domain.IsRegisteredCharity,
                CharityNumber = domain.CharityNumber,
                HasHcOrColGrant = domain.HasHcOrColGrant,
                HasHcvsOrHgOrAelGrant = domain.HasHcvsOrHgOrAelGrant,
                IsTraRegistered = domain.IsTraRegistered,
                RslOrHaAssociation = domain.RslOrHaAssociation,
                IsLotteryFunded = domain.IsLotteryFunded,
                LotteryFundedProject = domain.LotteryFundedProject,
                FundingOther = domain.FundingOther,
                HasChildSupport = domain.HasChildSupport,
                ChildSafeguardingLeadFirstName = domain.ChildSafeguardingLeadFirstName,
                ChildSafeguardingLeadLastName = domain.ChildSafeguardingLeadLastName,
                ChildSafeguardingLeadTrainingMonth = domain.ChildSafeguardingLeadTrainingMonth,
                ChildSafeguardingLeadTrainingYear = domain.ChildSafeguardingLeadTrainingYear,
                HasAdultSupport = domain.HasAdultSupport,
                HasAdultSafeguardingLead = domain.HasAdultSafeguardingLead,
                AdultSafeguardingLeadFirstName = domain.AdultSafeguardingLeadFirstName,
                AdultSafeguardingLeadLastName = domain.AdultSafeguardingLeadLastName,
                AdultSafeguardingLeadTrainingMonth = domain.AdultSafeguardingLeadTrainingMonth,
                AdultSafeguardingLeadTrainingYear = domain.AdultSafeguardingLeadTrainingYear,
                HasEnhancedSupport = domain.HasEnhancedSupport,
                IsLocalOfferListed = domain.IsLocalOfferListed
            };
        }
    }
}
