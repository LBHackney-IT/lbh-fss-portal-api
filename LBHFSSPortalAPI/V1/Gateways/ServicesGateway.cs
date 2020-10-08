using LBHFSSPortalAPI.V1.Boundary.Requests;
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

        public async Task<ServiceDomain> CreateService(ServiceRequest request)
        {
            if (request == null)
                throw new UseCaseException
                {
                    UserErrorMessage = "Could not add a new service, a valid 'create service' request was not provided"
                };

            if (!request.OrganisationId.HasValue)
                throw new UseCaseException()
                {
                    UserErrorMessage = "Service cannot be added as the provided organisation ID was not valid"
                };

            var organisation = await Context.Organisations
                .FirstOrDefaultAsync(o => o.Id == request.OrganisationId)
                .ConfigureAwait(false);

            if (organisation == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not find organisation ID '{request.OrganisationId}'"
                };

            var service = new Service()
            {
                Name = request.Name,
                Status = request.Status ?? ServiceStatus.Created,
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
                OrganisationId = request.OrganisationId,

                ServiceLocations = request?.Locations
                    .Select(l => new ServiceLocation()
                    {
                        Latitude = l.Latitude,
                        Longitude = l.Longitude,
                        Uprn = 0, //l.Uprn, DATABASE CHANGE REQUIRED - NEEDS TO BE A STRING!
                        Address1 = l.Address1,
                        City = l.City,
                        StateProvince = l.StateProvince,
                        PostalCode = l.PostalCode,
                        Country = l.Country
                    })
                    .ToList(),
                ServiceTaxonomies = new List<ServiceTaxonomy>(),
            };

            // business rule: if no service name is provided, default to the Organisation name
            if (string.IsNullOrWhiteSpace(service.Name))
            {
                service.Name = organisation.Name;
            }

            // Add the category taxonomies if provided

            var allTaxonomies = new List<ServiceTaxonomy>();

            if (request.Categories != null)
            {
                var newCategories = await CreateCategories(request.Categories, service).ConfigureAwait(false);

                if (newCategories.Any())
                    allTaxonomies.AddRange(newCategories);
            }

            // Add the demographic taxonomies if provided

            if (request.Demographics != null)
            {
                var newDemographics = await CreateDemographics(request.Demographics, service).ConfigureAwait(false);

                if (newDemographics.Any())
                    allTaxonomies.AddRange(newDemographics);
            }

            // associate new service with any provided categories & demographics
            if (allTaxonomies.Any())
                service.ServiceTaxonomies = allTaxonomies;

            try
            {
                Context.Services.Add(service);
                Context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                HandleDbUpdateException(e);
            }

            // Get fresh copy of the service from the database
            service = await GetServiceById(service.Id).ConfigureAwait(false);
            var serviceDomain = _mapper.ToDomain(service);

            return serviceDomain;
        }
        public async Task<ServiceDomain> GetServiceAsync(int serviceId)
        {
            var service = await GetServiceById(serviceId).ConfigureAwait(false);

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
            if (servicesQuery.Offset.HasValue)
                matchingServices = matchingServices.Skip(servicesQuery.Offset.Value);

            if (servicesQuery.Limit.HasValue)
                matchingServices = matchingServices.Take(servicesQuery.Limit.Value);

            try
            {
                var services = await matchingServices
                    .Where(s => s.Status != ServiceStatus.Deleted)
                    .Include(s => s.Organisation)
                    .ThenInclude(o => o.UserOrganisations)
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
        public async Task<ServiceDomain> UpdateService(ServiceRequest request, int serviceId)
        {
            if (request == null)
                throw new UseCaseException
                {
                    UserErrorMessage = "Could not add a new service, a valid 'create service' request was not provided"
                };

            if (!request.OrganisationId.HasValue)
                throw new UseCaseException()
                {
                    UserErrorMessage = "Service cannot be added as the provided organisation ID was not valid"
                };

            var organisation = await Context.Organisations
                .FirstOrDefaultAsync(o => o.Id == request.OrganisationId)
                .ConfigureAwait(false);

            if (organisation == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not find organisation ID {request.OrganisationId}"
                };

            var service = await GetServiceByIdWithTracking(serviceId).ConfigureAwait(false);

            if (service == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not find a service with the given ID {serviceId}"
                };

            service.Name = request.Name ?? service.Name;
            service.Status = request.Status ?? ServiceStatus.Updated;
            service.CreatedAt = request.CreatedAt ?? service.CreatedAt;
            service.UpdatedAt = DateTime.UtcNow;
            service.Description = request.Description ?? service.Description;
            service.Website = request.Website ?? service.Website;
            service.Email = request.Email ?? service.Email;
            service.Telephone = request.Telephone ?? service.Telephone;
            service.Facebook = request.Facebook ?? service.Facebook;
            service.Twitter = request.Twitter ?? service.Twitter;
            service.Instagram = request.Instagram ?? service.Instagram;
            service.Linkedin = request.Linkedin ?? service.Linkedin;
            service.Keywords = request.Keywords ?? service.Keywords;
            service.ReferralLink = request.ReferralLink ?? service.ReferralLink;
            service.ReferralEmail = request.ReferralEmail ?? service.ReferralEmail;

            service.Organisation = null;
            service.OrganisationId = request.OrganisationId ?? service.OrganisationId;

            if (request.Locations != null)
            {
                service.ServiceLocations = request.Locations
                    .Select(l => new ServiceLocation()
                    {
                        Latitude = l.Latitude,
                        Longitude = l.Longitude,
                        Uprn = 0, //l.Uprn, DATABASE CHANGE REQUIRED - NEEDS TO BE A STRING!
                        Address1 = l.Address1,
                        City = l.City,
                        StateProvince = l.StateProvince,
                        PostalCode = l.PostalCode,
                        Country = l.Country
                    })
                    .ToList();
            }

            // business rule: if no service name is provided, default to the Organisation name
            if (string.IsNullOrWhiteSpace(service.Name))
            {
                service.Name = organisation.Name;
            }

            // Add the category taxonomies if provided

            var currentTaxonomies = service.ServiceTaxonomies.ToList();

            if (request.Categories != null)
            {
                // Clear existing categories. Note: if an empty list of categories is sent, this is a valid
                // request to clear the current categories
                currentTaxonomies.RemoveAll(st => st.Taxonomy != null && st.Taxonomy.Vocabulary == TaxonomyVocabulary.Category);

                var newCategories = await CreateCategories(request.Categories, service).ConfigureAwait(false);

                if (newCategories.Any())
                    currentTaxonomies.AddRange(newCategories);
            }

            // Add the demographic taxonomies if provided

            if (request.Demographics != null)
            {
                // Clear existing demographics. Note: if an empty list of demographics is sent, this is a valid
                // request to clear the current demographics
                currentTaxonomies.RemoveAll(st => st.Taxonomy != null && st.Taxonomy.Vocabulary == TaxonomyVocabulary.Demographic);

                var newDemographics = await CreateDemographics(request.Demographics, service).ConfigureAwait(false);

                if (newDemographics.Any())
                    currentTaxonomies.AddRange(newDemographics);
            }

            // copy the collection of service taxonomies above (whether mofified or not) back to the service entity
            service.ServiceTaxonomies = currentTaxonomies;

            try
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException e)
            {
                HandleDbUpdateException(e);
            }

            // Get fresh copy of the service from the database
            service = await GetServiceById(service.Id).ConfigureAwait(false);
            var serviceDomain = _mapper.ToDomain(service);

            return serviceDomain;
        }

        public async Task DeleteService(int serviceId)
        {
            try
            {
                var service = await GetServiceById(serviceId).ConfigureAwait(false);
                service.Status = ServiceStatus.Deleted;
                Context.Update(service);
                await Context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException e)
            {
                HandleDbUpdateException(e);
            }
        }

        private async Task<Service> GetServiceById(int serviceId)
        {
            var service = await Context.Services
                .Where(s => s.Status != ServiceStatus.Deleted)
                .Include(s => s.Organisation)
                .ThenInclude(o => o.UserOrganisations)
                .ThenInclude(uo => uo.User)
                .Include(s => s.Image)
                .Include(s => s.ServiceLocations)
                .Include(s => s.ServiceTaxonomies)
                .ThenInclude(st => st.Taxonomy)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == serviceId)
                .ConfigureAwait(false);

            return service;
        }

        private async Task<Service> GetServiceByIdWithTracking(int serviceId)
        {
            var service = await Context.Services
                .Where(s => s.Status != ServiceStatus.Deleted)
                .Include(s => s.Organisation)
                .ThenInclude(o => o.UserOrganisations)
                .ThenInclude(uo => uo.User)
                .Include(s => s.Image)
                .Include(s => s.ServiceLocations)
                .Include(s => s.ServiceTaxonomies)
                .ThenInclude(st => st.Taxonomy)
                .SingleOrDefaultAsync(u => u.Id == serviceId)
                .ConfigureAwait(false);

            return service;
        }

        private async Task<List<ServiceTaxonomy>> CreateCategories(List<TaxonomyRequest> categories, Service service)
        {
            var taxonomies = new List<ServiceTaxonomy>();

            if (categories != null && categories.Any())
            {
                // Check the provided category taxonomy IDs exist before attempting to add

                var existingCategoryIds = await Context.Taxonomies
                    .Where(t => t.Vocabulary == TaxonomyVocabulary.Category)
                    .Select(t => t.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var requestedCategoryIds = categories.Select(c => c.Id).ToList();
                var idsNotFound = requestedCategoryIds.Where(i => existingCategoryIds.Contains(i) == false);

                if (idsNotFound.Any())
                {
                    var idList = string.Join(",", idsNotFound);
                    throw new UseCaseException()
                    {
                        UserErrorMessage = $"One or more of category IDs could not be found: {idList}"
                    };
                }

                taxonomies.AddRange(categories.Select(c => new ServiceTaxonomy()
                {
                    TaxonomyId = c.Id,
                    Service = service,
                    Description = c.Description
                })
                    .ToList());
            }

            return taxonomies;
        }

        private async Task<List<ServiceTaxonomy>> CreateDemographics(List<int> demographicIds, Service service)
        {
            var taxonomies = new List<ServiceTaxonomy>();

            if (demographicIds != null && demographicIds.Any())
            {
                // Check the provided demographic taxonomy IDs exist before attempting to add

                var existingDemographicIds = await Context.Taxonomies
                    .Where(t => t.Vocabulary == TaxonomyVocabulary.Demographic)
                    .Select(t => t.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var idsNotFound = demographicIds.Where(i => existingDemographicIds.Contains(i) == false);

                if (idsNotFound.Any())
                {
                    var idList = string.Join(",", idsNotFound);
                    throw new UseCaseException()
                    {
                        UserErrorMessage = $"One or more of the demographic IDs ({idList}) was not valid"
                    };
                }

                taxonomies.AddRange(demographicIds
                    .Select(c => new ServiceTaxonomy()
                    {
                        TaxonomyId = c,
                        Service = service,
                    })
                    .ToList());
            }

            return taxonomies;
        }
    }
}

