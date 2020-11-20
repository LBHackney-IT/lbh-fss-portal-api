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
                cfg.CreateMap<UserOrganisation, UserOrganisationDomain>();
                cfg.CreateMap<Organisation, OrganisationDomain>()
                    .ForMember(x => x.StatusMessage, opt => opt.Ignore());
                cfg.CreateMap<Taxonomy, TaxonomyDomain>();
                cfg.CreateMap<ServiceLocation, ServiceLocationDomain>();
                cfg.CreateMap<ServiceTaxonomy, ServiceTaxonomyDomain>();
                cfg.CreateMap<User, UserDomain>();
                cfg.CreateMap<UserRole, UserRoleDomain>();
                cfg.CreateMap<Role, RoleDomain>();
                cfg.CreateMap<OrganisationDomain, Organisation>()
                    .ForMember(x => x.Services, opt => opt.Ignore())
                    .ForMember(x => x.UserOrganisations, opt => opt.Ignore());
                cfg.CreateMap<UserDomain, User>()
                    .ForMember(x => x.Sessions, opt => opt.Ignore())
                    .ForMember(x => x.UserOrganisations, opt => opt.Ignore())
                    .ForMember(x => x.UserRoles, opt => opt.Ignore());
            });

            mapperConfig.AssertConfigurationIsValid();
            _mapper = new Mapper(mapperConfig);
        }

        public Organisation FromDomain(OrganisationDomain organisationDomain)
        {
            return _mapper.Map<Organisation>(organisationDomain);
        }

        public User FromDomain(UserDomain userDomain)
        {
            return _mapper.Map<User>(userDomain);
        }

        public UserOrganisationDomain ToDomain(UserOrganisation userOrganisation)
        {
            return _mapper.Map<UserOrganisationDomain>(userOrganisation);
        }

        public ServiceDomain ToDomain(Service service)
        {
            return _mapper.Map<ServiceDomain>(service);
        }

        public FileDomain ToDomain(File file)
        {
            return _mapper.Map<FileDomain>(file);
        }

        public OrganisationDomain ToDomain(Organisation org)
        {
            return _mapper.Map<OrganisationDomain>(org);
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

        public List<OrganisationDomain> ToDomain(IEnumerable<Organisation> orgs)
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
