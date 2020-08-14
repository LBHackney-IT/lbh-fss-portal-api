using LBHFSSPortalAPI.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IUsersGateway
    {
        List<UserDomain> GetAllUsers();
        //ResidentDomain GetResidentById(int id);
    }
}
