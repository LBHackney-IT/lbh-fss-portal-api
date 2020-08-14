using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.Factories;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class UsersGateway : IUsersGateway
    {
        private readonly UsersDatabaseContext _usersDatabaseContext;

        public UsersGateway(UsersDatabaseContext usersDatabaseContext)
        {
            _usersDatabaseContext = usersDatabaseContext;
        }

        public List<UserDomain> GetAllUsers()
        {
            var users = _usersDatabaseContext.Users.ToDomain();

            return users;
        }
    }
}
