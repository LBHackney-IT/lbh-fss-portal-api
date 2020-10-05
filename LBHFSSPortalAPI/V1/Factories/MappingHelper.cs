using AutoMapper;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.Factories
{
    /// <summary>
    /// Maps values from entity framework objects to domain objects
    /// </summary>
    public class MappingHelper
    {
        private readonly Mapper _mapper;

        public MappingHelper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Service, ServiceDomain>();
                cfg.CreateMap<File, FileDomain>();
                cfg.CreateMap<UserOrganization, UserOrganizationDomain>();
                cfg.CreateMap<Organization, OrganizationDomain>();
                cfg.CreateMap<Taxonomy, TaxonomyDomain>();
                cfg.CreateMap<ServiceLocation, ServiceLocationDomain>();
                cfg.CreateMap<ServiceTaxonomy, ServiceTaxonomyDomain>();
                cfg.CreateMap<User, UserDomain>();
                cfg.CreateMap<UserRole, UserRoleDomain>();
                cfg.CreateMap<Role, RoleDomain>();
            });

            mapperConfig.AssertConfigurationIsValid();
            _mapper = new Mapper(mapperConfig);
        }

        public ServiceDomain ToDomain(Service service)
        {
            return _mapper.Map<ServiceDomain>(service);
        }

        public FileDomain ToDomain(File file)
        {
            return _mapper.Map<FileDomain>(file);
        }

        public OrganizationDomain ToDomain(Organization org)
        {
            return _mapper.Map<OrganizationDomain>(org);
        }

        public ServiceLocationDomain ToDomain(ServiceLocation location)
        {
            return _mapper.Map<ServiceLocationDomain>(location);
        }

        public ServiceTaxonomyDomain ToDomain(ServiceTaxonomy taxonomy)
        {
            return _mapper.Map<ServiceTaxonomyDomain>(taxonomy);
        }

        public UserDomain ToDomain(User user)
        {
            return _mapper.Map<UserDomain>(user);
        }

        public List<ServiceDomain> ToDomain(IEnumerable<Service> services)
        {
            return services.Select(s => ToDomain(s)).ToList();
        }

        public List<FileDomain> ToDomain(IEnumerable<File> files)
        {
            return files.Select(s => ToDomain(s)).ToList();
        }

        public List<OrganizationDomain> ToDomain(IEnumerable<Organization> orgs)
        {
            return orgs.Select(s => ToDomain(s)).ToList();
        }

        public List<ServiceLocationDomain> ToDomain(IEnumerable<ServiceLocation> locations)
        {
            return locations.Select(s => ToDomain(s)).ToList();
        }

        public List<ServiceTaxonomyDomain> ToDomain(IEnumerable<ServiceTaxonomy> taxonomies)
        {
            return taxonomies.Select(s => ToDomain(s)).ToList();
        }

        public List<UserDomain> ToDomain(IEnumerable<User> users)
        {
            return users.Select(s => ToDomain(s)).ToList();
        }
    }
}
