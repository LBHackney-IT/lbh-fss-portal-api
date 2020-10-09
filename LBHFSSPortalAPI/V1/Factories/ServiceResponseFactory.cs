using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class ServiceResponseFactory
    {
        public static ServiceResponse ToResponse(this ServiceDomain domain)
        {
            var org = domain?.Organisation;
            var user = org?.UserOrganisations?.FirstOrDefault()?.User;

            var response = domain == null
                ? null
                : new ServiceResponse
                {
                    Id = domain.Id,
                    Name = domain.Name,
                    UserId = user?.Id,
                    UserName = user?.Name,
                    OrganisationId = org?.Id,
                    OrganisationName = org?.Name,
                    Status = domain.Status,
                    CreatedAt = domain.CreatedAt,
                    UpdatedAt = domain.UpdatedAt,
                    Description = domain.Description,
                    Website = domain.Website,
                    Email = domain.Email,
                    Telephone = domain.Telephone,
                    Facebook = domain.Facebook,
                    Twitter = domain.Twitter,
                    Instagram = domain.Instagram,
                    Linkedin = domain.Linkedin,
                    Keywords = domain.Keywords,
                    ReferralLink = domain.ReferralLink,
                    ReferralEmail = domain.ReferralEmail,
                    Locations = domain.ServiceLocations?
                        .Select(domain => new LocationResponse()
                        {
                            Latitude = domain.Latitude,
                            Longitude = domain.Longitude,
                            Uprn = domain.Uprn,
                            Address1 = domain.Address1,
                            City = domain.City,
                            StateProvince = domain.StateProvince,
                            PostalCode = domain.PostalCode,
                            Country = domain.Country
                        })
                        .ToList(),
                    Image = domain.Image == null
                    ? null
                    : new ImageResponse()
                    {
                        Id = domain.Image.Id,
                        Medium = domain.Image.Url,
                        Original = domain.Image.Url
                    },
                    Categories = domain.ServiceTaxonomies == null
                    ? null
                    : domain.ServiceTaxonomies
                        .Where(t => t.Taxonomy != null && t.Taxonomy.Vocabulary == TaxonomyVocabulary.Category)
                        .Select(t => new TaxonomyResponse
                        {
                            Id = t.Taxonomy.Id,
                            Name = t.Taxonomy.Name,
                            Description = t.Taxonomy.Description,
                            ServiceDescription = t.Description,
                            Vocabulary = t.Taxonomy.Vocabulary,
                            Weight = t.Taxonomy.Weight
                        }).ToList(),
                    Demographics = domain.ServiceTaxonomies == null
                    ? null
                    : domain.ServiceTaxonomies
                        .Where(t => t.Taxonomy != null && t.Taxonomy.Vocabulary == TaxonomyVocabulary.Demographic)
                        .Select(t => new TaxonomyResponse()
                        {
                            Id = t.Taxonomy.Id,
                            Name = t.Taxonomy.Name,
                            Description = t.Taxonomy.Description,
                            ServiceDescription = t.Description,
                            Vocabulary = t.Taxonomy.Vocabulary,
                            Weight = t.Taxonomy.Weight
                        })
                        .ToList()
                };

            return response;
        }

        public static List<ServiceResponse> ToResponse(this List<ServiceDomain> services)
        {
            return services.Select(s => ToResponse(s)).ToList();
        }
    }
}
