using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IAddressXRefGateway
    {
        Task<List<AddressXRef>> GetAddressXrefs(string uprn);
        string GetNHSNeighbourhood(string uprn);
    }
}
