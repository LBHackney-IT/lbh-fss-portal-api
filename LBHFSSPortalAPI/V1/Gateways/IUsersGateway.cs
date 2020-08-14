using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IUsersGateway
    {
        List<UserDomain> GetAllUsers();
    }
}
