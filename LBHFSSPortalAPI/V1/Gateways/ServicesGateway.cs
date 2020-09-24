using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class ServicesGateway : IServicesGateway
    {
        private readonly DatabaseContext _context;
        private readonly MappingHelper _mapper;

        public ServicesGateway(DatabaseContext databaseContext, MappingHelper map)
        {
            _context = databaseContext;
            _mapper = map;
        }

        public ServicesGateway(DatabaseContext databaseContext)
        {
            _context = databaseContext;
            _mapper = new MappingHelper();
        }

        public async Task<ServiceDomain> GetServiceAsync(int serviceId)
        {
            var service = await _context.Services
                .Include(s => s.Organization)
                .Include(s => s.ServiceLocations)
                .Include(s => s.ServiceTaxonomies)
                //.ThenInclude(st => st.Select(t => t.)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == serviceId)
                .ConfigureAwait(false);

            if (service != null)
                return _mapper.ToDomain(service);

            return null;
        }
    }
}
