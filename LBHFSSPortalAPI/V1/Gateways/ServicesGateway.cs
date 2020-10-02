using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class ServicesGateway : BaseGateway, IServicesGateway
    {
        private readonly MappingHelper _mapper;

        public ServicesGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }

        public Task<ServiceResponse> CreateService(CreateServiceRequest request)
        {
            if (request == null)
                throw new UseCaseException
                {
                    UserErrorMessage = "Could not add a new service, valid request was not provided"
                };

            var service = new Service()
            {
                Name = request.Name,
                Status = request.Status,
                CreatedAt = request.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = request.UpdatedAt,
                Description = request.Description,
                Website = request.Website,
                Email = request.Email,
                Telephone = request.Telephone,
                Facebook = request.Facebook,
                Twitter = request.Twitter,
                Instagram = request.Instagram,
                Linkedin = request.Linkedin,
                Keywords = request.Keywords,
                ReferralLink = request.ReferralLink,
                ReferralEmail = request.ReferralEmail,
                OrganizationId = request.OrganisationId,

                ServiceLocations = request?.Locations
                    .Select(l => new ServiceLocation()
                    {
                        Latitude = l.Latitude,
                        Longitude = l.Longitude,
                        Uprn = l.Uprn,
                        Address1 = l.Address1,
                        City = l.City,
                        StateProvince = l.StateProvince,
                        PostalCode = l.PostalCode,
                        Country = l.Country
                    })
                    .ToList(),
                ServiceTaxonomies = new List<ServiceTaxonomy>(),
            };

            var taxonomies = new List<ServiceTaxonomy>();

            if (request.Categories != null && request.Categories.Any())
            {
                taxonomies.AddRange(
                    request.Categories
                        .Select(c => new ServiceTaxonomy()
                        {
                            TaxonomyId = c.Id,
                            //D

                        })
                        .ToList());
            }

            if (request.Demographics != null && request.Demographics.Any())
            {
                taxonomies.AddRange(
                    request.Demographics
                        .Select(c => new ServiceTaxonomy()
                        {
                            TaxonomyId = c.Id
                        })
                        .ToList());
            }

            return null;
        }

        public async Task<ServiceDomain> GetServiceAsync(int serviceId)
        {
            var service = await Context.Services
                .Include(s => s.Organization)
                .ThenInclude(o => o.UserOrganizations)
                .ThenInclude(uo => uo.User)
                .Include(s => s.Image)
                .Include(s => s.ServiceLocations)
                .Include(s => s.ServiceTaxonomies)
                .ThenInclude(st => st.Taxonomy)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == serviceId)
                .ConfigureAwait(false);

            if (service != null)
                return _mapper.ToDomain(service);

            return null;
        }

        public async Task<List<ServiceDomain>> GetServicesAsync(ServicesQueryParam servicesQuery)
        {
            // Search       search term to use (searches on [name] column for the MVP)
            // Sort         the column name by which to sort
            // Direction    sort order; asc, desc
            // Limit        maximum number of records to return
            // Offset       number of records to skip for pagination

            List<ServiceDomain> response = new List<ServiceDomain>();
            var direction = ConvertToEnum(servicesQuery.Direction);

            if (direction == SortDirection.None)
                throw new UseCaseException()
                {
                    UserErrorMessage = "The sort direction was not valid (must be one of asc, desc)"
                };

            var matchingServices = Context.Services.AsQueryable();

            // handle search 
            if (!string.IsNullOrWhiteSpace(servicesQuery.Search))
                matchingServices = matchingServices.Where(s => EF.Functions.Like(s.Name, $"%{servicesQuery.Search}%"));

            // handle sort by column name and sort direction
            var entityPropName = GetEntityPropertyForColumnName(typeof(User), servicesQuery.Sort);

            if (entityPropName == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"The 'Sort' parameter contained the value '{servicesQuery.Sort}' " +
                                        "which is not a valid column name"
                };

            matchingServices = (direction == SortDirection.Asc) ?
                matchingServices.OrderBy(s => EF.Property<Service>(s, entityPropName)) :
                matchingServices.OrderByDescending(s => EF.Property<Service>(s, entityPropName));

            // handle pagination options
            if (servicesQuery.Limit.HasValue)
                matchingServices = matchingServices.Take(servicesQuery.Limit.Value);

            if (servicesQuery.Offset.HasValue)
                matchingServices = matchingServices.Skip(servicesQuery.Offset.Value);

            try
            {
                var services = await matchingServices
                    .Include(s => s.Organization)
                    .ThenInclude(o => o.UserOrganizations)
                    .ThenInclude(uo => uo.User)
                    .Include(s => s.Image)
                    .Include(s => s.ServiceLocations)
                    .Include(s => s.ServiceTaxonomies)
                    .ThenInclude(st => st.Taxonomy)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                response = _mapper.ToDomain(services);
            }
            catch (InvalidOperationException e)
            {
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not run the services search query with the supplied input parameters",
                    DevErrorMessage = e.Message
                };
            }

            return response;
        }
    }
}
