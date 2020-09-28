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
