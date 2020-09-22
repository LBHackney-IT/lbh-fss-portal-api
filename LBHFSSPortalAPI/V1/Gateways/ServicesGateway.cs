using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class ServicesGateway : IServicesGateway
    {
        private readonly DatabaseContext _context;

        public ServicesGateway(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<ServiceDomain> GetServiceAsync(int serviceId)
        {
            var service = await _context.Services.SingleOrDefaultAsync(u => u.Id == serviceId).ConfigureAwait(false);

            if (service != null)
                return service.ToDomain();

            return null;
        }
    }
}
