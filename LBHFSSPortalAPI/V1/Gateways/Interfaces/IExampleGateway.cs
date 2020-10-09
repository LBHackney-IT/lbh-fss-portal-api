using System.Collections.Generic;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
