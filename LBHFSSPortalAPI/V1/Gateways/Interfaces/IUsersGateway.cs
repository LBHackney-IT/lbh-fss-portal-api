using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IUsersGateway
    {
        UserDomain GetUser(string emailAddress, string status);
        List<UserDomain> GetAllUsers();
        UserDomain AddUser(UserDomain user);
        void UpdateUser(UserDomain user);
        UserDomain GetUserBySubId(string subId);
    }
}
