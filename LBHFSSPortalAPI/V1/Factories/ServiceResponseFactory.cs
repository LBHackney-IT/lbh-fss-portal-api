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
            var images = domain?.Image?.Url == null ? System.Array.Empty<string>() : domain.Image.Url.Split(';');
            var response = domain == null
                ? null
                : new ServiceResponse
                {
                    Id = domain.Id,
                    Name = domain.Name,
                    Users = ResponseFactory.formatUsers(domain.Organisation.UserOrganisations),
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
                            UPRN = domain.Uprn,
                            Address1 = domain.Address1,
                            Address2 = domain.Address2,
                            City = domain.City,
                            StateProvince = domain.StateProvince,
                            PostalCode = domain.PostalCode,
                            Country = domain.Country,
                            NHSNeighbourhood = domain.NHSNeighbourhood
                        })
                        .ToList(),
                    Image = domain.Image == null
                    ? null
                    : new ImageResponse()
                    {
                        Id = domain.Image.Id,
                        Medium = images.Length > 1 ? images[1] : null,
                        Original = images.Length > 0 ? images[0] : null
                    },
                    Categories = domain.ServiceTaxonomies == null
                    ? null
                    : domain.ServiceTaxonomies
                        .Where(t => t.Taxonomy != null && t.Taxonomy.Vocabulary == TaxonomyVocabulary.Category)
                        .Select(t => new TaxonomyResponse
                        {
                            Id = t.Taxonomy.Id,
                            Name = t.Taxonomy.Name,
                            Description = t.Description,
                            Vocabulary = t.Taxonomy.Vocabulary,
                            VocabularyId = 1,
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
                            Vocabulary = t.Taxonomy.Vocabulary,
                            VocabularyId = 2,
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
