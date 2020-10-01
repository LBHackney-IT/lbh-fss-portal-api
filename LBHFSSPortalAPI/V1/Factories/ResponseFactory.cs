using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class ResponseFactory
    {
        //TODO: Map the fields in the domain object(s) to fields in the response object(s).
        // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings
        public static ResponseObject ToResponse(this Entity domain)
        {
            return new ResponseObject();
        }

        public static List<ResponseObject> ToResponse(this IEnumerable<Entity> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }

        public static UserResponse ToResponse(this UserDomain domain)
        {
            var response = new UserResponse()
            {
                Id = domain.Id,
                Name = domain.Name,
                Email = domain.Email,
                Status = domain.Status,
                CreatedAt = domain.CreatedAt,
                SubId = domain.SubId,
                Organisation = domain.Organizations?.FirstOrDefault()?.ToResponse()
            };

            return response;
        }

        public static List<UserResponse> ToResponse(this IEnumerable<UserDomain> users)
        {
            return users.Select(p => p.ToResponse()).ToList();
        }

        public static OrganisationResponse ToResponse(this OrganizationDomain domain)
        {
            return new OrganisationResponse()
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
