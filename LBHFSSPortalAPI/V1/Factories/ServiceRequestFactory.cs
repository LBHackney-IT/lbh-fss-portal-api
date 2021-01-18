using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class ServiceRequestFactory
    {
        public static ServiceDomain ToDomain(this ServiceRequest request)
        {
            return new ServiceDomain()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                Status = request.Status,
                Description = request.Description,
                Email = request.Email,
                Facebook = request.Facebook,
                Instagram = request.Instagram,
                Keywords = request.Keywords,
                Linkedin = request.Linkedin,
                ReferralEmail = request.ReferralEmail,
                ReferralLink = request.ReferralLink,
                Telephone = request.Telephone,
                Twitter = request.Twitter,
                Website = request.Website,
                OrganisationId = request.OrganisationId,
                ServiceLocations = request.Locations.ToDomain()
            };
        }

        public static Service ToEntity(this ServiceRequest request)
        {
            return new Service()
            {
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                Status = request.Status,
                Description = request.Description,
                Email = request.Email,
                Facebook = request.Facebook,
                Instagram = request.Instagram,
                Keywords = request.Keywords,
                Linkedin = request.Linkedin,
                ReferralEmail = request.ReferralEmail,
                ReferralLink = request.ReferralLink,
                Telephone = request.Telephone,
                Twitter = request.Twitter,
                Website = request.Website,
                ServiceLocations = request.Locations.ToEntity()
            };
        }
    }
}
